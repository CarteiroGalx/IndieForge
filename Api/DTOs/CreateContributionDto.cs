using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieForge.DTOs
{
    public class CreateContributionDto
    {
        public Guid ProjetoId { get; set; }
        public decimal Valor { get; set; }
    }
}