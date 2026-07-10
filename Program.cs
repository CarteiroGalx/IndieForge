using IndieForge.Context;
using IndieForge.DTOs;
using IndieForge.Models;
using IndieForge.Models.Seeders;
using IndieForge.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IndieForge
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AdminService>();
            builder.Services.AddScoped<ContributionService>();
            builder.Services.AddScoped<ProjectService>();
            builder.Services.AddScoped<AccountService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
                await DatabaseSeeder.SeedAsync(context);
            }

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
                    return new LoginResponseDto("Nome de usuário ou senha inválidos");

                var hasher = new PasswordHasher<User>();
                var verification = hasher.VerifyHashedPassword(user, user.SenhaHash, loginDto.Password);

                if (verification == PasswordVerificationResult.Failed)
                    return new LoginResponseDto("Nome de usuário ou senha inválidos");

                var tokenString = await authService.Login(user);
                var response = new LoginResponseDto(tokenString);
                return response;
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

                var response = new ResponseMeDto
                {
                    Nome = user.Nome,
                    Email = user.Email,
                    EmailConfirmado = user.EmailConfirmado,
                    Projetos = await _context.Projects
                        .Where(p => p.IdCriador == userId)
                        .Select(p => new ProjectCardDto
                        {
                            Nome = p.Nome,
                            Descricao = p.Descricao,
                            Meta = p.MetaFinanceira,
                            Arrecadado = 23,
                            Status = p.Status,
                            DataCriacao = p.DataCriacao,
                            CriadorNome = user.Nome
                        })
                        .ToListAsync(),
                    Contribuicoes = await _context.Contribuicoes
                        .Where(c => c.UserId == userId)
                        .Include(c => c.Projeto)
                        .Select(c => new ContribuicaoDto
                        {
                            Valor = c.Valor,
                            DataContribuicao = c.DataCriacao,
                            projetoContribuido = new ProjectResumeDto
                            {
                                Nome = c.Projeto.Nome,
                                Meta = c.Projeto.MetaFinanceira,
                                Arrecadado = c.Projeto.Contribuicoes.Sum(contrib => contrib.Valor),
                                DataCriacao = c.Projeto.DataCriacao
                            }
                        })
                        .ToListAsync()
                };

                return Results.Ok(response);
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

            admin.MapPost("/projects/{id}/change-status", async (AppDbContext _context, Guid id, Status newStatus) =>
            {
                var project = await _context.Projects.FindAsync(id);
                if (project is null)
                    return Results.NotFound("Projeto não encontrado");

                project.Status = newStatus;
                await _context.SaveChangesAsync();

                return Results.Ok(project);
            });

            projects.MapGet("/", async (AppDbContext _context,
                                       string? name = "",
                                       bool maisArrecadado = false,
                                       decimal? minArrecadado = null,
                                       Status? status = null,
                                       string? ordenarPor = "criacao",
                                       bool desc = false) =>
            {
                IQueryable<Projeto> projects = _context.Projects
                        .Where(p => p.Status != Status.Oculto)
                        .Include(p => p.Criador)
                        .Include(p => p.Contribuicoes);

                if (!string.IsNullOrWhiteSpace(name))
                    projects = projects.Where(p => p.Nome.Contains(name));

                if (status.HasValue)
                    projects = projects.Where(p => p.Status == status.Value);

                // Projeção com soma das contribuições para permitir filtragem/ordenação por valor arrecadado
                var projected = projects
                    .Select(p => new
                    {
                        p,
                        Arrecadado = p.Contribuicoes.Sum(c => c.Valor)
                    });

                if (minArrecadado.HasValue)
                    projected = projected.Where(x => x.Arrecadado >= minArrecadado.Value);

                // Ordenação: prioridade para "maisArrecadado", senão usar ordenarPor (criacao, nome, meta)
                if (maisArrecadado)
                {
                    projected = projected.OrderByDescending(x => x.Arrecadado);
                }
                else
                {
                    bool asc = !desc;
                    projected = (ordenarPor ?? "criacao").ToLowerInvariant() 
                    switch
                    {
                        "nome" => asc ? projected.OrderBy(x => x.p.Nome) : projected.OrderByDescending(x => x.p.Nome),
                        "meta" or "metafinanceira" => asc ? projected.OrderBy(x => x.p.MetaFinanceira) : projected.OrderByDescending(x => x.p.MetaFinanceira),
                        _ => asc ? projected.OrderBy(x => x.p.DataCriacao) : projected.OrderByDescending(x => x.p.DataCriacao),
                    };
                }

                var projectsList = projected.Select(x => new ProjectCardDto
                {
                    Nome = x.p.Nome,
                    Descricao = x.p.Descricao,
                    Meta = x.p.MetaFinanceira,
                    Arrecadado = x.Arrecadado,
                    Status = x.p.Status,
                    DataCriacao = x.p.DataCriacao,
                    CriadorNome = x.p.Criador.Nome,
                });

                return await projectsList.ToListAsync();
            });

            admin.MapGet("/contributions", async (AppDbContext _context) =>
            {
                var contributions = await _context.Contribuicoes.ToListAsync();
                return contributions;
            });

            projects.MapPost("/", async (AppDbContext _context, ClaimsPrincipal user, CreateProjectDto dto) =>
            {
                var userIdFromToken = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdFromToken, out var idCriador))
                    return Results.BadRequest();

                var project = new Projeto
                {
                    IdCriador = idCriador,
                    Nome = dto.Nome,
                    Descricao = dto.Descricao,
                    MetaFinanceira = dto.MetaFinanceira,
                    Status = Status.Ativo
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return Results.Created($"/api/projects/{project.Id}", new { project.Id, project.Nome, project.Descricao, project.MetaFinanceira, project.Status });
            });

            me.MapPatch("/change-password", async (AppDbContext _context, ClaimsPrincipal acess, ChangePasswordDto dto) =>
            {
                var userIdFromToken = acess.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdFromToken, out var userId))
                    return Results.BadRequest();

                var user = await _context.Users.FindAsync(userId);
                if (user is null) return Results.NotFound("Usuário não encontrado");

                if (dto.SenhaAtual != dto.SenhaConfirmacao) return Results.BadRequest("As senhas devem coincidirem");

                var hash = new PasswordHasher<User>();
                user.SenhaHash = hash.HashPassword(user, dto.NovaSenha);

                await _context.SaveChangesAsync();

                return Results.Ok("Senha alterada com sucesso!");
            });

            me.MapPost("/reset-password/", async (AppDbContext _context, string tokenString, string newPassword) =>
            {
                var token = await _context.PasswordRecuperationTokens.FirstOrDefaultAsync(t => t.Token == tokenString);
                if (token is null) return Results.BadRequest("Token inválido");

                if (token.Used) return Results.BadRequest("Token inválido");

                if (token.ExpiresAt < DateTime.UtcNow) return Results.BadRequest("Token inválido");

                var user = await _context.Users.FindAsync(token.UserId);

                if (user is null) return Results.BadRequest("Token inválido");

                var hasher = new PasswordHasher<User>();

                user.SenhaHash = hasher.HashPassword(user, newPassword);
                token.Used = true;
                _context.PasswordRecuperationTokens.Remove(token);
                await _context.SaveChangesAsync();

                return Results.Ok();
            });

            me.MapPost("/forgot-password", async (AppDbContext _context, string email) =>
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                var tokenString = "";
                if (user != null)
                {
                    var oldTokens = await _context.PasswordRecuperationTokens
                        .Where(t => t.UserId == user.Id && !t.Used)
                        .ToListAsync();

                    tokenString = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));

                    _context.PasswordRecuperationTokens.RemoveRange(oldTokens);

                    var passwordRecoverToken = new PasswordRecuperationToken
                    {
                        UserId = user.Id,
                        Token = tokenString,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                        Used = false
                    };

                    _context.PasswordRecuperationTokens.Add(passwordRecoverToken);
                    await _context.SaveChangesAsync();
                }

                return new
                {
                    Token = tokenString,
                    Info = "ATENÇÃO: Num ambiente de produção real, a resposta da API não deve ser dessa forma. " +
                    "O token é liberado aqui para facilitar os testes da API."
                };
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
                        p.Contribuicoes.Sum(c => c.Valor),
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

            projects.MapPost("/alterar-meta", async (AppDbContext _context, ChangeMetaFinanceiraDto dto, ClaimsPrincipal acess) =>
            {
                var userIdFromToken = acess.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdFromToken, out var userId)) return Results.Unauthorized();

                var projeto = await _context.Projects.FindAsync(dto.ProjetoId);
                if (projeto is null) return Results.NotFound();
                if (projeto.TotalContribuicoes > 0)
                    return Results.BadRequest("Você não pode alterar a meta de um projeto que já tenha contribuições!");
                
                projeto.MetaFinanceira = dto.NovoValor;
                await _context.SaveChangesAsync();
                return Results.Ok("Meta alterada com sucesso!");
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
                var arrecadadoAntes = projeto.TotalArrecadado;

                projeto.TotalArrecadado += dto.Valor;
                projeto.TotalContribuicoes++;

                var metaFoiAtingida =
                    arrecadadoAntes < projeto.MetaFinanceira &&
                    projeto.TotalArrecadado >= projeto.MetaFinanceira;
                if (metaFoiAtingida)
                {
                    projeto.Status = Status.EncerradoPorMeta;
                }
                await _context.SaveChangesAsync();
                if (metaFoiAtingida)
                {
                    return Results.Ok(new
                    {
                        Message = "Você foi quem finalizou a meta. Parabéns!"
                    });
                }
                return Results.Ok();
            }).RequireAuthorization();

            app.Run();
        }
    }
}
