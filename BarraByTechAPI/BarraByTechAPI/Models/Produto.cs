using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json; // Necessário para a serialização/desserialização das imagens

namespace BarraByTechAPI.Models
{
    public class Produto
    {
        [Key]
        public Guid ProdutoId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        // --- Relacionamentos de Categorias e Marcas ---

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "A marca é necessária!")]
        public Guid MarcaId { get; set; }

        [Required(ErrorMessage = "A Tipo é necessária!")]
        public Guid TipoMarcaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        // --- Propriedades de Estoque e Preço ---

        [Range(0, int.MaxValue, ErrorMessage = "O estoque mínimo não pode ser negativo.")]
        public int EstoqueMin { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque atual não pode ser negativo.")]
        public int EstoqueAtual { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor do boleto deve ser maior ou igual a zero.")]
        public decimal Boleto { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor do cartão deve ser maior ou igual a zero.")]
        public decimal Cartao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor à vista deve ser maior ou igual a zero.")]
        public decimal Vista { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor promocional deve ser maior ou igual a zero.")]
        public decimal Promocao { get; set; }

        // --- Data e Métodos de Controle ---

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAlteracao { get; set; }

        public void AtualizarDataAlteracao() => DataAlteracao = DateTime.UtcNow;
    }
}