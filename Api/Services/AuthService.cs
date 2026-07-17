using IndieForge.Context;
using IndieForge.DTOs;
using IndieForge.Models;
using Microsoft.AspNetCore.Identity;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IndieForge.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Registrar(RegisterDto register)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Nome = register.UserName,
                Email = register.Email
            };

            user.SenhaHash = hasher.HashPassword(user, register.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Guid.NewGuid().ToString(); // Retorna um token fictício para fins de demonstração
        }

        public async Task<string> Login(User user)
        {
            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            if (string.IsNullOrEmpty(key)) throw new InvalidOperationException("JWT key not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(tokenDescriptor);
            
            return tokenString;
        }

        public async Task StatusCheck()
        {
            
        }

        internal async Task<string> GerarTokenConfirmacaoEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new InvalidOperationException("Usuário não encontrado");
            }
            
            var token = Guid.NewGuid().ToString();
            var emailToken = new EmailConfirmationToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24) // Token expira em 24 horas
            };

            _context.EmailConfirmationTokens.Add(emailToken);
            await _context.SaveChangesAsync();

            return token;
        }
    }
}