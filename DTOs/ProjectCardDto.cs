using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndieForge.Models;

namespace IndieForge.DTOs
{
    public class ProjectCardDto(string nome, string descricao, decimal meta, decimal arrecadado, Status status, DateTime dataCriacao, string criadorNome)
    {
        public string Nome { get; set; } = nome;
        public string Descricao { get; set; } = descricao;
        public decimal Meta { get; set; } = meta;
        public decimal Arrecadado { get; set; } = arrecadado;
        public DateTime DataCriacao { get; set; } = dataCriacao;
        public Status Status { get; set; } = status;
        public string CriadorNome { get; set; } = criadorNome;
    }
}