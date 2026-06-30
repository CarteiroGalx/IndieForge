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

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Deve ser um email válido")]
        [StringLength(256, ErrorMessage = "O email não pode ter mais de 256 caracteres")]
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public List<Projeto> Projetos { get; set; } = [];
        public List<Contribuicao> Contribuicoes { get; set; } = [];

        [Required(ErrorMessage = "O papel do usuário é obrigatório")]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
