namespace IndieForge.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Projeto> Projecos { get; set; } = [];
        public UserRole Role { get; set; } = UserRole.User;
    }
}
