using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieForge.Models
{
    public class Contribuicao
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("User")]
        public Guid UserId { get; set; } 
        public User User { get; set; } = null!;
        [ForeignKey("Projeto")]
        public Guid ProjectId { get; set; }
        public Projeto Projeto { get; set; } = null!;
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public Contribuicao() { }
        public Contribuicao(Guid userId, Guid projectId, decimal valor)
        {
            UserId = userId;
            ProjectId = projectId;
            Valor = valor;
        }
    }
}
