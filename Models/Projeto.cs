namespace IndieForge.Models
{
    public enum Status
    {
        Ativo,
        Oculto,
        Encerrado
    }

    public class Projeto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdCriador { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal MetaFinanceira { get; set; }
        public List<Contribuicao> Contribuicoes { get; set; } = [];
        public Status Status { get; set; } = Status.Ativo;
    }
}
