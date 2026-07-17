using System.ComponentModel.DataAnnotations;

namespace IndieForge.Models
{
    public class PasswordRecuperationToken
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public bool Used { get; set; } = false;

    }
}
