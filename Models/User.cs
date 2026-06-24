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
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Deve ser um email válido")]
        [StringLength(256, ErrorMessage = "O email não pode ter mais de 256 caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(500, MinimumLength = 20, ErrorMessage = "O hash da senha deve ter entre 20 e 500 caracteres")]
        [Display(Name = "Senha")]
        public string SenhaHash { get; set; } = string.Empty;

        [Display(Name = "Projetos")]
        public List<Projeto> Projetos { get; set; } = [];

        [Display(Name = "Contribuições")]
        public List<Contribuicao> Contribuicoes { get; set; } = [];

        [Required(ErrorMessage = "O papel do usuário é obrigatório")]
        [Display(Name = "Papel")]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
