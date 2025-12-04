using System;
using System.ComponentModel.DataAnnotations;

namespace BarraByTechAPI.Models
{
    public class EnderecoCliente
    {
        [Key]
        public Guid EnderecoId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public Guid UserId { get; set; }

        public Cliente? Cliente { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [Range(10000000, 99999999, ErrorMessage = "O CEP deve ter 8 dígitos.")]
        public int Cep { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(200, ErrorMessage = "O endereço pode ter no máximo 200 caracteres.")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(100, ErrorMessage = "A cidade pode ter no máximo 100 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(50, ErrorMessage = "O estado pode ter no máximo 50 caracteres.")]
        public string Estado { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "O complemento pode ter no máximo 100 caracteres.")]
        public string Complemento { get; set; } = string.Empty;
    }
}