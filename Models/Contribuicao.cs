namespace IndieForge.Models
{
    public class Contribuicao
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Projeto Projeto { get; set; } = null!;
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
