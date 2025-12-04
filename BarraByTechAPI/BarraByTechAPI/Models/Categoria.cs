using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class Categoria
    {
        [Key]
        public Guid CategoriaId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(80, ErrorMessage = "O nome da categoria deve ter no máximo 80 caracteres.")]
        public string CategoriaNome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição da categoria é obrigatória.")]
        [StringLength(150, ErrorMessage = "A descrição da categoria deve ter no máximo 150 caracteres.")]
        public string CategoriaDescricao { get; set; } = string.Empty;
    }
}