using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class ItensFavoritos
    {
        [Key]
        public Guid ItemFavoritoId { get; set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public Guid ProdutoId { get; set; }
    }
}
