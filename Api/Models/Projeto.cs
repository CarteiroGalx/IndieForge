using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieForge.Models
{
    public enum Status
    {
        Ativo,
        Finalizado,
        Oculto,
        Cancelado,
        EncerradoPeloCriador
    }

    public class Projeto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey(nameof(Criador))]
        public Guid IdCriador { get; set; }
        public User Criador { get; set; } = null!;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal MetaFinanceira { get; set; }
        public List<Contribuicao> Contribuicoes { get; set; } = [];
        public int TotalContribuicoes { get; set; }
        public decimal TotalArrecadado { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataConclusao { get; set; }
        public Status Status { get; set; } = Status.Ativo;
        public decimal Percentage => (TotalArrecadado / MetaFinanceira) * 100;
    }
}
