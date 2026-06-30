using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class ProjectResponseDto(string nome, string descricao, decimal metaFinanceira, int totalContribuicoes, decimal totalArrecadado, Status status, DateTime data)
    {
        public string Nome { get; set; } = nome;
        public string Descricao { get; set; } = descricao;
        public decimal MetaFinanceira { get; set; } = metaFinanceira;
        public int TotalContribuicoes { get; set; } = totalContribuicoes;
        public decimal TotalArrecadado { get; set; } = totalArrecadado;
        public Status Status { get; set; } = status;
        public DateTime DataCriacao { get; set; } = data;
    }
}