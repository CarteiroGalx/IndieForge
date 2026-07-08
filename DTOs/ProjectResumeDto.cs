using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndieForge.DTOs
{
    public class ProjectResumeDto
    {
        public string Nome { get; set; }
        public decimal Meta { get; set; }
        public decimal Arrecadado { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}