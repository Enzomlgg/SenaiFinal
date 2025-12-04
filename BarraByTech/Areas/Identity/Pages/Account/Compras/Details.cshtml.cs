using BarraByTech.Models.DTO;
using BarraByTech.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BarraByTech.Areas.Identity.Pages.Account.Compras
{
    public class DetailsModel : PageModel
    {
        private readonly ClienteService _clienteService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DetailsModel(ClienteService clienteService, IWebHostEnvironment hostEnvironment)
        {
            _clienteService = clienteService;
            _hostEnvironment = hostEnvironment;
        }

        public ComprasDTO Compra { get; set; } = new();
        public EnderecoClienteDTO? EnderecoEntrega { get; set; }
        public List<ItemCompraDetalheDTO> ItensDetalhes { get; set; } = new();

        public class ItemCompraDetalheDTO
        {
            public string NomeProduto { get; set; } = string.Empty;
            public string ImagemUrl { get; set; } = string.Empty;
            public int Quantidade { get; set; }
            public decimal ValorUnitario { get; set; }
            public decimal Total => Quantidade * ValorUnitario;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound();

            var compra = await _clienteService.GetPagamentoAsync(id);
            if (compra == null) return NotFound();
            Compra = compra;

            if (Compra.EnderecoId != Guid.Empty)
            {
                EnderecoEntrega = await _clienteService.GetEnderecoByIdAsync(Compra.EnderecoId);
            }

            var itensPedido = await _clienteService.GetItensPedidoAsync(compra.PedidoId);

            var produtoTasks = itensPedido
                .Select(item => _clienteService.GetProdutoById(item.ProdutoId))
                .ToList();

            var produtos = await Task.WhenAll(produtoTasks);

            var detalhes = new List<ItemCompraDetalheDTO>();

            foreach (var (item, produto) in itensPedido.Zip(produtos, (item, produto) => (item, produto)))
            {
                if (produto == null) continue;

                string produtoIdStr = produto.ProdutoId.ToString();
                string pastaFisica = Path.Combine(_hostEnvironment.WebRootPath, "Imagens", "Produtos", produtoIdStr);
                string pastaVirtual = $"/Imagens/Produtos/{produtoIdStr}/";

                string imagem = "/img/placeholder.png";

                if (Directory.Exists(pastaFisica))
                {
                    string[] extensoes = { "*.png", "*.jpg", "*.jpeg", "*.webp" };
                    foreach (var ext in extensoes)
                    {
                        var arquivos = Directory.GetFiles(pastaFisica, ext).OrderBy(f => f);
                        if (arquivos.Any())
                        {
                            imagem = pastaVirtual + Path.GetFileName(arquivos.First());
                            break;
                        }
                    }
                }

                detalhes.Add(new ItemCompraDetalheDTO
                {
                    NomeProduto = produto.Nome,
                    ImagemUrl = imagem,
                    Quantidade = item.Quantidade,
                    ValorUnitario = produto.Cartao
                });
            }

            ItensDetalhes = detalhes;

            return Page();
        }
    }
}