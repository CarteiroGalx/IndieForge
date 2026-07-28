namespace IndieForge.DTOs
{
    public class ContributionResponseDto
    {
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
        public string NomeApoiador { get; set; }

        public ContributionResponseDto() { }

        public ContributionResponseDto(decimal valor, DateTime dataCriacao, string nomeApoiador)
        {
            Valor = valor;
            DataCriacao = dataCriacao;
            NomeApoiador = nomeApoiador;
        }
    }
}