namespace APIEletrica.Models
{
    public class PedidoItem
    {
        public long Id { get; set; }
        public long IdPedido { get; set; }
        public long IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;
    }
}
