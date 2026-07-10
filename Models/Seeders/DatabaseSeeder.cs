using System;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;
using IndieForge.Context;
using Microsoft.AspNetCore.Identity;

namespace IndieForge.Models.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Se já houver usuários, assume que o banco já foi populado
            if (context.Users.Any()) return;

            var hasher = new PasswordHasher<User>();

            // Usuários estáticos
            var users = new[]
            {
                new User { Id = Guid.Parse("11111111-1111-4111-8111-111111111111"), Nome = "Admin", Email = "admin@indieforge.local", EmailConfirmado = true, SenhaHash = string.Empty, Role = UserRole.Admin },
                new User { Id = Guid.Parse("22222222-2222-4222-8222-222222222222"), Nome = "João Silva", Email = "joao.silva@example.com", EmailConfirmado = true, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("33333333-3333-4333-8333-333333333333"), Nome = "Mariana Costa", Email = "mariana.costa@example.com", EmailConfirmado = false, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("44444444-4444-4444-8444-444444444444"), Nome = "Lucas Pereira", Email = "lucas.pereira@example.com", EmailConfirmado = true, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("55555555-5555-4555-8555-555555555555"), Nome = "Ana Oliveira", Email = "ana.oliveira@example.com", EmailConfirmado = false, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("66666666-6666-4666-8666-666666666666"), Nome = "Pedro Santos", Email = "pedro.santos@example.com", EmailConfirmado = true, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("77777777-7777-4777-8777-777777777777"), Nome = "Carla Mendes", Email = "carla.mendes@example.com", EmailConfirmado = false, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("88888888-8888-4888-8888-888888888888"), Nome = "Rafael Gomes", Email = "rafael.gomes@example.com", EmailConfirmado = true, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("99999999-9999-4999-8999-999999999999"), Nome = "Beatriz Almeida", Email = "beatriz.almeida@example.com", EmailConfirmado = false, SenhaHash = string.Empty, Role = UserRole.User },
                new User { Id = Guid.Parse("aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa"), Nome = "Thiago Ribeiro", Email = "thiago.ribeiro@example.com", EmailConfirmado = true, SenhaHash = string.Empty, Role = UserRole.User }
            };

            // Hash das senhas (User@1 ... User@10)
            for (var i = 0; i < users.Length; i++)
            {
                users[i].SenhaHash = hasher.HashPassword(users[i], $"User@{i+1}");
            }

            // Projetos estáticos — pertencem a usuários acima (IDs fixos)
            var projects = new[]
            {
                new Projeto { Id = Guid.Parse("10101010-1010-4101-8101-101010101010"), IdCriador = Guid.Parse("11111111-1111-4111-8111-111111111111"), Criador = null!, Nome = "Projeto Alpha", Descricao = "Projeto Alpha - um exemplo estático.", MetaFinanceira = 5000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-1), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("20202020-2020-4202-8202-202020202020"), IdCriador = Guid.Parse("22222222-2222-4222-8222-222222222222"), Criador = null!, Nome = "Projeto Beta", Descricao = "Projeto Beta - um exemplo estático.", MetaFinanceira = 10000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-2), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("30303030-3030-4303-8303-303030303030"), IdCriador = Guid.Parse("33333333-3333-4333-8333-333333333333"), Criador = null!, Nome = "Projeto Gamma", Descricao = "Projeto Gamma - um exemplo estático.", MetaFinanceira = 2500m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-3), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("40404040-4040-4404-8404-404040404040"), IdCriador = Guid.Parse("44444444-4444-4444-8444-444444444444"), Criador = null!, Nome = "Projeto Delta", Descricao = "Projeto Delta - um exemplo estático.", MetaFinanceira = 7500m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-4), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("50505050-5050-4505-8505-505050505050"), IdCriador = Guid.Parse("55555555-5555-4555-8555-555555555555"), Criador = null!, Nome = "Projeto Épsilon", Descricao = "Projeto Épsilon - um exemplo estático.", MetaFinanceira = 3000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-5), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("60606060-6060-4606-8606-606060606060"), IdCriador = Guid.Parse("66666666-6666-4666-8666-666666666666"), Criador = null!, Nome = "Projeto Zeta", Descricao = "Projeto Zeta - um exemplo estático.", MetaFinanceira = 20000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-6), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("70707070-7070-4707-8707-707070707070"), IdCriador = Guid.Parse("77777777-7777-4777-8777-777777777777"), Criador = null!, Nome = "Projeto Eta", Descricao = "Projeto Eta - um exemplo estático.", MetaFinanceira = 1500m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-7), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("80808080-8080-4808-8808-808080808080"), IdCriador = Guid.Parse("88888888-8888-4888-8888-888888888888"), Criador = null!, Nome = "Projeto Theta", Descricao = "Projeto Theta - um exemplo estático.", MetaFinanceira = 12000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-8), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("90909090-9090-4909-8909-909090909090"), IdCriador = Guid.Parse("99999999-9999-4999-8999-999999999999"), Criador = null!, Nome = "Projeto Iota", Descricao = "Projeto Iota - um exemplo estático.", MetaFinanceira = 6000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-9), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("a0a0a0a0-a0a0-4a0a-8a0a-a0a0a0a0a0a0"), IdCriador = Guid.Parse("aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa"), Criador = null!, Nome = "Projeto Kappa", Descricao = "Projeto Kappa - um exemplo estático.", MetaFinanceira = 4000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-10), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("b1b1b1b1-b1b1-4b1b-8b1b-b1b1b1b1b1b1"), IdCriador = Guid.Parse("11111111-1111-4111-8111-111111111111"), Criador = null!, Nome = "Projeto Lambda", Descricao = "Projeto Lambda - um exemplo estático.", MetaFinanceira = 8000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-11), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("c2c2c2c2-c2c2-4c2c-8c2c-c2c2c2c2c2c2"), IdCriador = Guid.Parse("22222222-2222-4222-8222-222222222222"), Criador = null!, Nome = "Projeto Mu", Descricao = "Projeto Mu - um exemplo estático.", MetaFinanceira = 9000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-12), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("d3d3d3d3-d3d3-4d3d-8d3d-d3d3d3d3d3d3"), IdCriador = Guid.Parse("33333333-3333-4333-8333-333333333333"), Criador = null!, Nome = "Projeto Nu", Descricao = "Projeto Nu - um exemplo estático.", MetaFinanceira = 11000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-13), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("e4e4e4e4-e4e4-4e4e-8e4e-e4e4e4e4e4e4"), IdCriador = Guid.Parse("44444444-4444-4444-8444-444444444444"), Criador = null!, Nome = "Projeto Xi", Descricao = "Projeto Xi - um exemplo estático.", MetaFinanceira = 13000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-14), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("f5f5f5f5-f5f5-4f5f-8f5f-f5f5f5f5f5f5"), IdCriador = Guid.Parse("55555555-5555-4555-8555-555555555555"), Criador = null!, Nome = "Projeto Omicron", Descricao = "Projeto Omicron - um exemplo estático.", MetaFinanceira = 7000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-15), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("01010101-0101-4101-8101-010101010101"), IdCriador = Guid.Parse("66666666-6666-4666-8666-666666666666"), Criador = null!, Nome = "Projeto Pi", Descricao = "Projeto Pi - um exemplo estático.", MetaFinanceira = 14000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-16), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("12121212-1212-4121-8121-121212121212"), IdCriador = Guid.Parse("77777777-7777-4777-8777-777777777777"), Criador = null!, Nome = "Projeto Rho", Descricao = "Projeto Rho - um exemplo estático.", MetaFinanceira = 16000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-17), Status = Status.Ativo },
                new Projeto { Id = Guid.Parse("13131313-1313-4131-8131-131313131313"), IdCriador = Guid.Parse("88888888-8888-4888-8888-888888888888"), Criador = null!, Nome = "Projeto Sigma", Descricao = "Projeto Sigma - um exemplo estático.", MetaFinanceira = 18000m, TotalContribuicoes = 0, TotalArrecadado = 0m, DataCriacao = DateTime.UtcNow.AddDays(-18), Status = Status.Ativo }
            };

            // Contribuições estáticas (valores fixos) — criadas via inicializador para manter Ids estáticos
            var contribs = new[]
            {
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000001"), UserId = Guid.Parse("22222222-2222-4222-8222-222222222222"), ProjectId = Guid.Parse("10101010-1010-4101-8101-101010101010"), Valor = 150m, DataCriacao = DateTime.UtcNow.AddMonths(-1) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000002"), UserId = Guid.Parse("33333333-3333-4333-8333-333333333333"), ProjectId = Guid.Parse("20202020-2020-4202-8202-202020202020"), Valor = 200m, DataCriacao = DateTime.UtcNow.AddMonths(-2) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000003"), UserId = Guid.Parse("44444444-4444-4444-8444-444444444444"), ProjectId = Guid.Parse("20202020-2020-4202-8202-202020202020"), Valor = 350m, DataCriacao = DateTime.UtcNow.AddMonths(-3) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000004"), UserId = Guid.Parse("55555555-5555-4555-8555-555555555555"), ProjectId = Guid.Parse("40404040-4040-4404-8404-404040404040"), Valor = 75m, DataCriacao = DateTime.UtcNow.AddMonths(-4) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000005"), UserId = Guid.Parse("66666666-6666-4666-8666-666666666666"), ProjectId = Guid.Parse("50505050-5050-4505-8505-505050505050"), Valor = 25m, DataCriacao = DateTime.UtcNow.AddMonths(-5) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000006"), UserId = Guid.Parse("77777777-7777-4777-8777-777777777777"), ProjectId = Guid.Parse("50505050-5050-4505-8505-505050505050"), Valor = 50m, DataCriacao = DateTime.UtcNow.AddMonths(-6) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000007"), UserId = Guid.Parse("88888888-8888-4888-8888-888888888888"), ProjectId = Guid.Parse("50505050-5050-4505-8505-505050505050"), Valor = 100m, DataCriacao = DateTime.UtcNow.AddMonths(-7) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000008"), UserId = Guid.Parse("99999999-9999-4999-8999-999999999999"), ProjectId = Guid.Parse("70707070-7070-4707-8707-707070707070"), Valor = 500m, DataCriacao = DateTime.UtcNow.AddMonths(-8) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000009"), UserId = Guid.Parse("aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa"), ProjectId = Guid.Parse("80808080-8080-4808-8808-808080808080"), Valor = 1000m, DataCriacao = DateTime.UtcNow.AddMonths(-9) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-00000000000A"), UserId = Guid.Parse("11111111-1111-4111-8111-111111111111"), ProjectId = Guid.Parse("80808080-8080-4808-8808-808080808080"), Valor = 250m, DataCriacao = DateTime.UtcNow.AddMonths(-10) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-00000000000B"), UserId = Guid.Parse("22222222-2222-4222-8222-222222222222"), ProjectId = Guid.Parse("a0a0a0a0-a0a0-4a0a-8a0a-a0a0a0a0a0a0"), Valor = 60m, DataCriacao = DateTime.UtcNow.AddMonths(-11) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-00000000000C"), UserId = Guid.Parse("33333333-3333-4333-8333-333333333333"), ProjectId = Guid.Parse("b1b1b1b1-b1b1-4b1b-8b1b-b1b1b1b1b1b1"), Valor = 420m, DataCriacao = DateTime.UtcNow.AddMonths(-12) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-00000000000D"), UserId = Guid.Parse("44444444-4444-4444-8444-444444444444"), ProjectId = Guid.Parse("c2c2c2c2-c2c2-4c2c-8c2c-c2c2c2c2c2c2"), Valor = 300m, DataCriacao = DateTime.UtcNow.AddMonths(-13) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-00000000000E"), UserId = Guid.Parse("55555555-5555-4555-8555-555555555555"), ProjectId = Guid.Parse("c2c2c2c2-c2c2-4c2c-8c2c-c2c2c2c2c2c2"), Valor = 150m, DataCriacao = DateTime.UtcNow.AddMonths(-14) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-00000000000F"), UserId = Guid.Parse("66666666-6666-4666-8666-666666666666"), ProjectId = Guid.Parse("e4e4e4e4-e4e4-4e4e-8e4e-e4e4e4e4e4e4"), Valor = 800m, DataCriacao = DateTime.UtcNow.AddMonths(-15) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000010"), UserId = Guid.Parse("77777777-7777-4777-8777-777777777777"), ProjectId = Guid.Parse("f5f5f5f5-f5f5-4f5f-8f5f-f5f5f5f5f5f5"), Valor = 100m, DataCriacao = DateTime.UtcNow.AddMonths(-16) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000011"), UserId = Guid.Parse("88888888-8888-4888-8888-888888888888"), ProjectId = Guid.Parse("f5f5f5f5-f5f5-4f5f-8f5f-f5f5f5f5f5f5"), Valor = 150m, DataCriacao = DateTime.UtcNow.AddMonths(-17) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000012"), UserId = Guid.Parse("99999999-9999-4999-8999-999999999999"), ProjectId = Guid.Parse("f5f5f5f5-f5f5-4f5f-8f5f-f5f5f5f5f5f5"), Valor = 250m, DataCriacao = DateTime.UtcNow.AddMonths(-18) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000013"), UserId = Guid.Parse("aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa"), ProjectId = Guid.Parse("01010101-0101-4101-8101-010101010101"), Valor = 60m, DataCriacao = DateTime.UtcNow.AddMonths(-19) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000014"), UserId = Guid.Parse("11111111-1111-4111-8111-111111111111"), ProjectId = Guid.Parse("13131313-1313-4131-8131-131313131313"), Valor = 500m, DataCriacao = DateTime.UtcNow.AddMonths(-20) },
                new Contribuicao { Id = Guid.Parse("10000000-0000-4000-8000-000000000015"), UserId = Guid.Parse("22222222-2222-4222-8222-222222222222"), ProjectId = Guid.Parse("13131313-1313-4131-8131-131313131313"), Valor = 750m, DataCriacao = DateTime.UtcNow.AddMonths(-21) }
            };

            // Ajusta totais dos projetos com base nas contribuições (agora em memória)
            foreach (var c in contribs)
            {
                var p = projects.FirstOrDefault(pj => pj.Id == c.ProjectId);
                if (p == null) continue;
                p.TotalContribuicoes += 1;
                p.TotalArrecadado += c.Valor;
            }

            // Adiciona tudo ao contexto e salva uma única vez
            context.Users.AddRange(users);
            context.Projects.AddRange(projects);
            context.Contribuicoes.AddRange(contribs);

            await context.SaveChangesAsync();
        }
    }
}
