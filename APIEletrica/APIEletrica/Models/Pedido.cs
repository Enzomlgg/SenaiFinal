namespace APIEletrica.Models
{
    public class Pedido
    {
        public long Id { get; set; }
        public long IdCliente { get; set; }
        public DateTime DataHora { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pendente";
        public DateTime? DataEntregaPrevista { get; set; }
        public string Observacoes { get; set; }
    }
}
