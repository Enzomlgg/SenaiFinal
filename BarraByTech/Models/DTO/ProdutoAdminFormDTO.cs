namespace BarraByTech.Models.DTO
{
    public class ProdutoAdminFormDTO
    {
        // Campos que vêm do formulário via FormData
        public Guid? ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Guid? CategoriaId { get; set; }
        public Guid? MarcaId { get; set; }
        public Guid? TipoMarcaId { get; set; }

        // Preços
        public decimal Boleto { get; set; }
        public decimal Cartao { get; set; }
        public decimal Vista { get; set; }
        public decimal Promocao { get; set; }
    }
}
