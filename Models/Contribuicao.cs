using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndieForge.Models
{
    public class Contribuicao
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "O usuário é obrigatório")]
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "O ID do projeto é obrigatório")]
        [ForeignKey("Projeto")]
        public Guid ProjectId { get; set; }

        [Required(ErrorMessage = "O projeto é obrigatório")]
        public Projeto Projeto { get; set; } = null!;

        [Required(ErrorMessage = "O valor da contribuição é obrigatório")]
        [Range(0.01, 100_000_000, ErrorMessage = "O valor deve ser maior que zero")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Valor { get; set; }

        [Display(Name = "Data de Criação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
