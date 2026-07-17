using System.ComponentModel.DataAnnotations;

namespace IndieForge.DTOs
{
    public class EditProjectDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "A descrição do projeto é obrigatória")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 2000 caracteres")]
        public string Description { get; set; } = string.Empty;
    }
}
