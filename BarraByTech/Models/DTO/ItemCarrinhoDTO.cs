namespace BarraByTech.Models.DTO
{
    public class ItemCarrinhoDTO
    {
        public Guid ItemCarrinhoId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }

}
