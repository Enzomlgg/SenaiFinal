using BarraByTech.Models.DTO;

namespace BarraByTech.Models
{
    public class ProdutoDetalheViewModel
    {
        // Propriedades vindas do ProdutoDTO
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public int EstoqueAtual { get; set; }

        public decimal Boleto { get; set; }
        public decimal Cartao { get; set; }
        public decimal Vista { get; set; }
        public decimal Promocao { get; set; }

        // IDs necessários para buscar os nomes
        public Guid MarcaId { get; set; }
        public Guid TipoMarcaId { get; set; }

        // Propriedades para Exibição (Serão preenchidas no Controller/PageModel)
        public string MarcaNome { get; set; } = string.Empty;
        public string TipoMarcaNome { get; set; } = string.Empty;

        // Propriedades de Visualização
        public Dictionary<string, decimal> Precos { get; set; } = new Dictionary<string, decimal>();
        public List<string> ImagensUrl { get; set; } = new List<string>();

        // Construtor principal
        public ProdutoDetalheViewModel(ProdutoDTO dto)
        {
            ProdutoId = dto.ProdutoId;
            Nome = dto.Nome;
            Descricao = dto.Descricao;
            EstoqueAtual = dto.EstoqueAtual;

            Boleto = dto.Boleto;
            Cartao = dto.Cartao;
            Vista = dto.Vista;
            Promocao = dto.Promocao;

            MarcaId = dto.MarcaId;
            TipoMarcaId = dto.TipoMarcaId;

            Precos = new Dictionary<string, decimal>();

            // 1. PRIORIDADE: PROMOÇÃO
            if (dto.Promocao > 0)
            {
                Precos.Add("Promoção PIX / À Vista", dto.Promocao);
            }

            // 2. PIX / À VISTA
            if (dto.Vista > 0)
            {
                if (dto.Promocao == 0)
                {
                    Precos.Add("Pix / À Vista", dto.Vista);
                }
                else if (dto.Vista != dto.Promocao && !Precos.ContainsKey("Pix / À Vista Simples"))
                {
                    Precos.Add("Pix / À Vista Simples", dto.Vista);
                }
            }

            // 3. BOLETO
            if (dto.Boleto > 0 && !Precos.ContainsKey("Boleto"))
            {
                Precos.Add("Boleto", dto.Boleto);
            }

            // 4. CARTÃO
            if (dto.Cartao > 0 && !Precos.ContainsKey("Cartão de Crédito"))
            {
                Precos.Add("Cartão de Crédito", dto.Cartao);
            }
        }

        public ProdutoDetalheViewModel() { }
    }
}