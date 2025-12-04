using BarraByTech.Models.DTO;
using System.Collections.Generic;

namespace BarraByTech.Models
{
    public class ProdutoViewModel
    {
        public ProdutoDTO Produto { get; set; } = new ProdutoDTO();
        public List<CategoriaDTO> Categorias { get; set; } = new List<CategoriaDTO>();
        public List<MarcaDTO> Marcas { get; set; } = new List<MarcaDTO>();
        public List<TiposMarcaDTO> TiposMarca { get; set; } = new List<TiposMarcaDTO>();
        public List<ComprasDTO> Compras { get; set; } = new List<ComprasDTO>();
    }
}
