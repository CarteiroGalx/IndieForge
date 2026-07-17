using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class ProjectDetailsDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal MetaFinanceira { get; set; }
        public int TotalContribuicoes { get; set; }
        public decimal TotalArrecadado { get; set; }
        public Status Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<ContributionResponseDto> Contribuicoes { get; set; } = new List<ContributionResponseDto>();
        public decimal Porcentagem { get; set; }

        public ProjectDetailsDto(string nome, string descricao, decimal metaFinanceira, int totalContribuicoes, decimal totalArrecadado, Status status, DateTime data, List<ContributionResponseDto> contribuicoes, decimal porcentagem)
        {
            Nome = nome;
            Descricao = descricao;
            MetaFinanceira = metaFinanceira;
            TotalContribuicoes = totalContribuicoes;
            TotalArrecadado = totalArrecadado;
            Status = status;
            DataCriacao = data;
            Contribuicoes = contribuicoes;
            Porcentagem = porcentagem;
        }

        public ProjectDetailsDto(string nome, string descricao, decimal metaFinanceira, int totalContribuicoes, decimal totalArrecadado, Status status, DateTime data)
        {
            Nome = nome;
            Descricao = descricao;
            MetaFinanceira = metaFinanceira;
            TotalContribuicoes = totalContribuicoes;
            TotalArrecadado = totalArrecadado;
            Status = status;
            DataCriacao = data;
        }
    }
}