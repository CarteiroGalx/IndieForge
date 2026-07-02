namespace IndieForge.DTOs
{
    public class ContributionResponseDto(decimal valor, DateTime dataCriacao, string nomeApoiador)
    {
        public decimal Valor { get; set; } = valor;
        public DateTime DataCriacao { get; set; } = dataCriacao;
        public string NomeApoiador { get; set; } = nomeApoiador;
    }
}