namespace BarraByTech.Models.DTO
{
    public class CategoriaDTO
    {
        public Guid CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public string CategoriaDescricao { get; set; } = string.Empty;
    }
}
