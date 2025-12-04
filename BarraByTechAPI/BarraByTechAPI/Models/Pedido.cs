using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarraByTechAPI.Models
{
    public class Pedido
    {
        [Key]
        public Guid PedidoId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "A data do pedido é obrigatória.")]
        public DateTime Data { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Cliente? Cliente { get; set; }
    }
}
