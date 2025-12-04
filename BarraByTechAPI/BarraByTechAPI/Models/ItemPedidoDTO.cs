namespace BarraByTechAPI.Models
{
    public class ItemPedidoDTO
    {
        public Guid ItemPedidoId { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
