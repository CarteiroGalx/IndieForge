using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieForge.DTOs
{
    public class ResponseMeDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmado { get; set; }
        public List<ProjectCardDto> Projetos { get; set; }
        public List<ContribuicaoDto> Contribuicoes { get; set; }
        public decimal TotalArrecadadoEmContribuicoes => Contribuicoes?.Sum(c => c.Valor) ?? 0;
    }
}