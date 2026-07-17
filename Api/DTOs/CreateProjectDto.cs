using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieForge.DTOs
{
    public class CreateProjectDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal MetaFinanceira { get; set; }
    }
}