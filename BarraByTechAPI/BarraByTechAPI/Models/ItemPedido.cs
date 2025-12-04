using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class ItemPedido
    {
        [Key]
        public Guid ItemPedidoId { get; set; } = Guid.NewGuid();

        // FK para Pedido
        [Required(ErrorMessage = "O PedidoId é obrigatório.")]
        public Guid PedidoId { get; set; }

        [ForeignKey(nameof(PedidoId))]
        public Pedido? Pedido { get; set; }

        // FK para Produto
        [Required(ErrorMessage = "O ProdutoId é obrigatório.")]
        public Guid ProdutoId { get; set; }

        [ForeignKey(nameof(ProdutoId))]
        public Produto? Produto { get; set; }

        // Quantidade
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        public int Quantidade { get; set; } = 1;
    }
}
