using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieForge.Models
{
    public enum Status
    {
        Ativo,
        Oculto,
        Cancelado,
        EncerradoPorMeta,
        EncerradoPeloCriador
    }

    public class Projeto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O ID do criador é obrigatório")]
        [ForeignKey(nameof(Criador))]
        public Guid IdCriador { get; set; }

        [Required(ErrorMessage = "O criador do projeto é obrigatório")]
        public User Criador { get; set; } = null!;

        [Required(ErrorMessage = "O nome do projeto é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do projeto é obrigatória")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 2000 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A meta financeira é obrigatória")]
        [Range(0.01, 999_999_999, ErrorMessage = "A meta deve ser maior que zero")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MetaFinanceira { get; set; }
        public List<Contribuicao> Contribuicoes { get; set; } = [];
        public int TotalContribuicoes { get; set; }
        public decimal TotalArrecadado { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "O status é obrigatório")]
        public Status Status { get; set; } = Status.Ativo;
    }
}
