using BarraByTech.Models.Enums;

namespace BarraByTech.Models.DTO
{
    public class ComprasDTO
    {
        public Guid ComprasId { get; set; }
        public Guid PedidoId { get; set; }
        public Guid EnderecoId { get; set; }
        public MetodosPagamento Metodo { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
        public StatusPagamento Status { get; set; }
    }
}
