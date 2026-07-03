using IndieForge.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IndieForge.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Projeto> Projects { get; set; }
        public DbSet<Contribuicao> Contribuicoes { get; set; }
        public DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Criador)
                .WithMany(u => u.Projetos)
                .HasForeignKey(p => p.IdCriador);

            modelBuilder.Entity<Contribuicao>()
                .HasOne(c => c.User)
                .WithMany(u => u.Contribuicoes)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Contribuicao>()
                .HasOne(c => c.Projeto)
                .WithMany(p => p.Contribuicoes)
                .HasForeignKey(c => c.ProjectId);
        }
    }
}
