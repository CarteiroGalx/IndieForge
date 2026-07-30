using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class ProjectCardDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Meta { get; set; }
        public decimal Arrecadado { get; set; }
        public decimal Percentage { get; set; }
        public DateTime DataCriacao { get; set; }
        public Status Status { get; set; }
        public string CriadorNome { get; set; }
    }
}