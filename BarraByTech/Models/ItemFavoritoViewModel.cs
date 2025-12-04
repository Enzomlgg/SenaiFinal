namespace BarraByTech.Models
{
    public class ItemFavoritoViewModel
    {
        public Guid ItemFavoritoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public decimal Vista { get; set; }
        public decimal Promocao { get; set; }
        public string ImagemUrl { get; set; }
    }
}