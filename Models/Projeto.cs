using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieForge.Models
{
    public enum Status
    {
        Ativo,
        Oculto,
        Encerrado
    }

    public class Projeto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O ID do criador é obrigatório")]
        [ForeignKey("Criador")]
        public Guid IdCriador { get; set; }

        [Required(ErrorMessage = "O criador do projeto é obrigatório")]
        public User Criador { get; set; } = null!;

        [Required(ErrorMessage = "O nome do projeto é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        [Display(Name = "Nome do Projeto")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do projeto é obrigatória")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 2000 caracteres")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A meta financeira é obrigatória")]
        [Range(0.01, 100_000_000, ErrorMessage = "A meta deve ser maior que zero")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Meta Financeira")]
        public decimal MetaFinanceira { get; set; }

        [Display(Name = "Contribuições")]
        public List<Contribuicao> Contribuicoes { get; set; } = [];

        [Required(ErrorMessage = "O status é obrigatório")]
        [Display(Name = "Status")]
        public Status Status { get; set; } = Status.Ativo;
    }
}
