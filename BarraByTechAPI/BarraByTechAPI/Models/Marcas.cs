using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class Marcas
    {
        [Key]
        public Guid MarcaId { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
    }
}
