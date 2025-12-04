using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class Compras
    {
        [Key]
        public Guid ComprasId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O pedido é obrigatório.")]
        public Guid PedidoId { get; set; }

        [ForeignKey(nameof(PedidoId))]
        public Pedido? Pedido { get; set; }

        [Required(ErrorMessage = "O método de pagamento é obrigatório.")]
        public MetodosPagamento Metodo { get; set; }
        public Guid EnderecoId { get; set; }

        [Required(ErrorMessage = "O valor pago é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal ValorPago { get; set; }

        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;

        [Required]
        public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;
    }
}
public enum MetodosPagamento
{
    Boleto,
    Cartao,
    Vista,
    Promocao
}

public enum StatusPagamento
{
    Pendente,
    Aprovado,
    Recusado,
    Cancelado,
    Estornado,
    Expirado
}