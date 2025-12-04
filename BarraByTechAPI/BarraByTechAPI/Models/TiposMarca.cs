using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class TiposMarca
    {
        [Key]
        public Guid TipoMarcaId { get; set; } = Guid.NewGuid();
        public Guid MarcaId { get; set; }
        public string Nome { get; set; }
    }
}
