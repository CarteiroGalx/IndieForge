
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IndieForge.Context;
using IndieForge.DTOs;
using IndieForge.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace IndieForge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Exemplo", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT no formato: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=app.db"));
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // AGRUPAMENTO POR ROTAS
            var admin = app.MapGroup("/api/admin");
            var projects = app.MapGroup("/api/projects");
            var auth = app.MapGroup("/api/auth");
            //-------

            app.MapGet("/api/ping", () => "Pong!");

            app.MapPost("/api/login", async (AppDbContext _context, LoginDto loginDto) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Nome == loginDto.UserName);
                if (user is null)
                {
                    throw new InvalidOperationException("Nome de usuário ou senha inválidos");
                }

                var hasher = new PasswordHasher<User>();
                var verification = hasher.VerifyHashedPassword(user, user.SenhaHash, loginDto.Password);
                if (verification == PasswordVerificationResult.Failed)
                {
                    throw new InvalidOperationException("Nome de usuário ou senha inválidos");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var key = builder.Configuration["Jwt:Key"];
                var issuer = builder.Configuration["Jwt:Issuer"];
                var audience = builder.Configuration["Jwt:Audience"];
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

            });

            app.MapPost("/api/register", async (AppDbContext _context, RegisterDto registerDto) =>
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Nome == registerDto.UserName);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Nome de usuário já existe");
                }

                var hasher = new PasswordHasher<User>();
                var user = new User
                {
                    Nome = registerDto.UserName,
                    Email = registerDto.Email,
                    SenhaHash = hasher.HashPassword(null, registerDto.Password),
                    Role = registerDto.Role
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return "Usuário registrado com sucesso";
            });

            app.MapGet("/api/users", async (AppDbContext _context) =>
            {
                var users = await _context.Users.ToListAsync();
                return users;
            });

            app.MapGet("/api/projects", async (AppDbContext _context) =>
            {
                var projects = await _context.Projects
                        .Select(p => new ProjectResponseDto(
                            p.Nome,
                            p.Descricao,
                            p.MetaFinanceira,
                            p.TotalContribuicoes,
                            p.TotalArrecadado,
                            p.Status,
                            p.DataCriacao
                        ))
                        .ToListAsync();
                return projects;
            });

            app.MapGet("/api/contributions", async (AppDbContext _context) =>
            {
                var contributions = await _context.Contribuicoes.ToListAsync();
                return contributions;
            });

            app.MapPost("/api/projects", async (AppDbContext _context, Guid idCriador, string nome, string descricao, decimal metaFinanceira) =>
            {
                var project = new Projeto
                {
                    IdCriador = idCriador,
                    Nome = nome,
                    Descricao = descricao,
                    MetaFinanceira = metaFinanceira,
                    Status = Status.Ativo
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return project;
            });

            app.MapGet("/api/project/{id}", async (AppDbContext _context, string id) =>
            {
                if (!Guid.TryParse(id, out var projectId))
                    return Results.BadRequest("Id inválido.");

                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                var response = new ProjectResponseDto(
                    project.Nome,
                    project.Descricao,
                    project.MetaFinanceira,
                    project.TotalContribuicoes,
                    project.TotalArrecadado,
                    project.Status,
                    project.DataCriacao
                );

                return Results.Ok(response);
            });

            app.MapPost("/api/teste", async (AppDbContext _context, ClaimsPrincipal user, CreateContributionDto request) =>
            {
                var userIdFromToken = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(userIdFromToken, out var userId))
                    return Results.Unauthorized();

                var userEntity = await _context.Users.FindAsync(userId);

                if (userEntity is null)
                    return Results.NotFound("Usuário não encontrado");

                var project = await _context.Projects.FindAsync(request.ProjetoId);

                if (project is null)
                    return Results.NotFound(new { message = "Projeto não encontrado"});

                var contribution = new Contribuicao(
                    userId,
                    request.ProjetoId,
                    request.Valor
                );
                _context.Contribuicoes.Add(contribution);
                project.TotalArrecadado += request.Valor;

                await _context.SaveChangesAsync();

                return Results.Ok("Deu certo!");
            });

            app.Run();
        }
    }
}
