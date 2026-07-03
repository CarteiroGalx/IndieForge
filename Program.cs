using IndieForge.Context;
using IndieForge.DTOs;
using IndieForge.Models;
using IndieForge.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace IndieForge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminOnly", policy =>
                    {
                        policy.RequireRole("Admin");
                    });
                });

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

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<ContributionService>();
            builder.Services.AddScoped<ProjectService>();
            builder.Services.AddScoped<AccountService>();

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
            var admin = app.MapGroup("/api/admin").RequireAuthorization("AdminOnly");
            var projects = app.MapGroup("/api/projects");
            var auth = app.MapGroup("/api/auth");
            var me = app.MapGroup("/api/me").RequireAuthorization();
            //-------

            app.MapGet("/api/ping", () => "Pong!");

            app.MapGet("/api/check-auth", (ClaimsPrincipal user) => 
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = user.FindFirst(ClaimTypes.Name)?.Value;
                var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

                return new
                {
                    UserId = userId,
                    UserName = userName,
                    UserRole = userRole
                };
            }).RequireAuthorization();

            auth.MapPost("/login", async (AppDbContext _context, LoginDto loginDto, AuthService authService) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Nome == loginDto.UserName);

                if (user is null)
                    throw new InvalidOperationException("Nome de usuário ou senha inválidos");

                var hasher = new PasswordHasher<User>();
                var verification = hasher.VerifyHashedPassword(user, user.SenhaHash, loginDto.Password);

                if (verification == PasswordVerificationResult.Failed)
                    throw new InvalidOperationException("Nome de usuário ou senha inválidos");

                return await authService.Login(user);

            });

            auth.MapPost("/register", async (AppDbContext _context, RegisterDto registerDto, AuthService authService) =>
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Nome == registerDto.UserName);

                if (existingUser != null)
                    throw new InvalidOperationException("Nome de usuário já existe");

                var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

                if (existingEmail != null)
                    throw new InvalidOperationException("Email ja cadastrado");

                await authService.Registrar(registerDto);
                var token = await authService.GerarTokenConfirmacaoEmail(registerDto.Email);

                return new { Token = token, Message = "Usuário registrado com sucesso", Info = "Utilize este token simulado para confirmar seu email" };
            });

            me.MapGet("/", async (AppDbContext _context, ClaimsPrincipal acess) =>
            {
                var userIdFromToken = acess.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdFromToken, out var userId))
                    return Results.BadRequest();
                
                var user = await _context.Users.FindAsync(userId);
                if (user is null)
                    return Results.NotFound("Usuário não encontrado");
                
                return Results.Ok(new
                {
                    user.Nome,
                    user.Email,
                    user.EmailConfirmado,
                    user.Role
                });
            });

            me.MapGet("/confirm-email", async (AppDbContext _context, string token) =>
            {
                var confirmationToken = await _context.EmailConfirmationTokens.FirstOrDefaultAsync(t => t.Token == token);
                if (confirmationToken == null || confirmationToken.Used || confirmationToken.ExpiresAt <= DateTime.UtcNow)
                {
                    return Results.NotFound("Token inválido ou expirado");
                }

                var user = await _context.Users.FindAsync(confirmationToken.UserId);
                if (user == null)
                {
                    return Results.NotFound("Usuário não encontrado");
                }

                user.EmailConfirmado = true;
                confirmationToken.Used = true;

                await _context.SaveChangesAsync();
                return Results.Ok("Email confirmado com sucesso");
            });

            admin.MapGet("/projects", async (AppDbContext _context) =>
            {
                return await _context.Projects.ToListAsync();
            });

            projects.MapGet("/", async (AppDbContext _context) =>
            {
                var projects = await _context.Projects
                        .Where(p => p.Status != Status.Oculto)
                        .Include(c => c.Criador)
                        .Include(p => p.Contribuicoes)
                        .Select(p => new ProjectCardDto(
                            p.Nome,
                            p.Descricao,
                            p.MetaFinanceira,
                            (decimal)(p.Contribuicoes.Sum(c => (double?)c.Valor) ?? 0d),
                            p.Status,
                            p.DataCriacao,
                            p.Criador.Nome
                        ))
                        .ToListAsync();
                return projects;
            });

            admin.MapGet("/contributions", async (AppDbContext _context) =>
            {
                var contributions = await _context.Contribuicoes.ToListAsync();
                return contributions;
            });

            projects.MapPost("/", async (AppDbContext _context, Guid idCriador, string nome, string descricao, decimal metaFinanceira) =>
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

            projects.MapGet("/{id}", async (AppDbContext _context, Guid id) =>
            {
                var response = await _context.Projects
                    .Where(p => p.Id == id && p.Status != Status.Oculto)
                    .Include(p => p.Contribuicoes)
                        .ThenInclude(c => c.User)
                    .Select(p => new ProjectDetailsDto(
                        p.Nome,
                        p.Descricao,
                        p.MetaFinanceira,
                        p.Contribuicoes.Count(),
                        (decimal)(p.Contribuicoes.Sum(c => (double?)c.Valor) ?? 0d),
                        p.Status,
                        p.DataCriacao,
                        p.Contribuicoes
                            .OrderByDescending(c => c.DataCriacao)
                            .Select(c => new ContributionResponseDto(
                                c.Valor,
                                c.DataCriacao,
                                c.User.Nome
                            ))
                            .ToList()
                    ))
                    .FirstOrDefaultAsync();

                if (response is null)
                    return Results.NotFound("Projeto não encontrado");

                return Results.Ok(response);
            });

            projects.MapPost("/contribution", async (AppDbContext _context, CreateContributionDto dto, ClaimsPrincipal user) =>
            {
                var userIdFromToken = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdFromToken, out var userId))
                    return Results.BadRequest();

                var projeto = await _context.Projects.FindAsync(dto.ProjetoId);
                if (projeto is null) return Results.NotFound("Projeto não encontrado");

                var cont = new Contribuicao(
                        userId,
                        projeto.Id,
                        dto.Valor
                    );

                _context.Contribuicoes.Add(cont);
                await _context.SaveChangesAsync();

                return Results.Ok();
            }).RequireAuthorization();

            app.Run();
        }
    }
}
