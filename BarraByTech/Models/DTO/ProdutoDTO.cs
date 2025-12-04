namespace BarraByTech.Models.DTO
{
    public class ProdutoDTO
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public Guid MarcaId { get; set; }
        public string Tipo { get; set; } = string.Empty;

        public int EstoqueAtual { get; set; }
        public int EstoqueMin { get; set; }

        public decimal Boleto { get; set; }
        public decimal Cartao { get; set; }
        public decimal Vista { get; set; }
        public decimal Promocao { get; set; }

        public Guid CategoriaId { get; set; }
        public Guid TipoMarcaId { get; set; }
    }
}