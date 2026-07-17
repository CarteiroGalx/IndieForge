using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class ContribuicaoDto
    {
        public decimal Valor { get; set; }
        public DateTime DataContribuicao { get; set; }
        public ProjectResumeDto projetoContribuido { get; set; }
    }
}