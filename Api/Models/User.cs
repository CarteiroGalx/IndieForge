using System.ComponentModel.DataAnnotations;

namespace IndieForge.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmado { get; set; } = false;
        public string SenhaHash { get; set; } = string.Empty;
        public List<Projeto> Projetos { get; set; } = [];
        public List<Contribuicao> Contribuicoes { get; set; } = [];
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
