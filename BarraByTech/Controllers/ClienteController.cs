using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BarraByTech.Models;
using System.Security.Claims;
using BarraByTech.Services;
using System.Collections.Generic;
using BarraByTech.Models.DTO;
using BarraByTech.Models.Enums;

namespace BarraByTech.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteService _clienteService;
        private readonly AdminService _adminService;
        private readonly IWebHostEnvironment _env;

        public ClienteController(ClienteService clienteService, AdminService adminService, IWebHostEnvironment env)
        {
            _clienteService = clienteService;
            _adminService = adminService;
            _env = env;
        }

        public IActionResult Integrantes()
        {
            return View();
        }

        // ================= ENDEREÇOS =================
        public async Task<IActionResult> Enderecos()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var enderecos = await _clienteService.GetEnderecosDoUsuarioAsync(usuarioId);
            return View(enderecos);
        }

        public IActionResult CriarEndereco() => View(new EnderecoClienteDTO());

        [HttpPost]
        public async Task<IActionResult> CriarEndereco(EnderecoClienteDTO dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            dto.UserId = usuarioId;
            var criado = await _clienteService.CreateEnderecoAsync(dto);
            if (!criado) return BadRequest("Não foi possível criar o endereço.");
            return RedirectToAction("Enderecos");
        }

        public async Task<IActionResult> EditarEndereco(Guid id)
        {
            var endereco = await _clienteService.GetEnderecoByIdAsync(id);
            if (endereco == null) return NotFound();
            return View(endereco);
        }

        [HttpPost]
        public async Task<IActionResult> EditarEndereco(EnderecoClienteDTO dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            dto.UserId = usuarioId;
            var atualizado = await _clienteService.UpdateEnderecoAsync(dto);
            if (!atualizado) return BadRequest("Não foi possível atualizar o endereço.");
            return RedirectToAction("Enderecos");
        }

        [HttpPost]
        public async Task<IActionResult> RemoverEndereco(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var sucesso = await _clienteService.DeleteEnderecoAsync(id, usuarioId);
            if (!sucesso) return BadRequest("Não foi possível remover o endereço.");
            return RedirectToAction("Enderecos");
        }

        // ================= FAVORITOS =================
        public async Task<IActionResult> Favoritos()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var meusFavoritos = (await _clienteService.GetFavoritos())
                                .Where(f => f.UsuarioId == usuarioId)
                                .ToList();

            var favoritosDetalhes = new List<ItemFavoritoViewModel>();

            foreach (var favorito in meusFavoritos)
            {
                var produtoDto = await _adminService.GetProdutoById(favorito.ProdutoId);
                if (produtoDto != null)
                {
                    string pastaVirtual = $"/Imagens/Produtos/{produtoDto.ProdutoId}/";
                    string pastaFisica = System.IO.Path.Combine(_env.WebRootPath, "Imagens", "Produtos", produtoDto.ProdutoId.ToString());
                    string imagemUrl = "/img/placeholder.png";

                    if (System.IO.Directory.Exists(pastaFisica))
                    {
                        string basePng = System.IO.Path.Combine(pastaFisica, "Base.png");
                        if (System.IO.File.Exists(basePng)) imagemUrl = pastaVirtual + "Base.png";
                        else
                        {
                            string[] extensoes = { "png", "jpg", "jpeg", "webp" };
                            foreach (var ext in extensoes)
                            {
                                var arquivos = System.IO.Directory.GetFiles(pastaFisica, $"*.{ext}");
                                if (arquivos.Length > 0)
                                {
                                    imagemUrl = pastaVirtual + System.IO.Path.GetFileName(arquivos[0]);
                                    break;
                                }
                            }
                        }
                    }

                    favoritosDetalhes.Add(new ItemFavoritoViewModel
                    {
                        ItemFavoritoId = favorito.ItemFavoritoId,
                        ProdutoId = favorito.ProdutoId,
                        NomeProduto = produtoDto.Nome,
                        DescricaoProduto = produtoDto.Descricao,
                        Vista = produtoDto.Vista,
                        Promocao = produtoDto.Promocao,
                        ImagemUrl = imagemUrl
                    });
                }
            }

            return View(favoritosDetalhes);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarFavorito(Guid produtoId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var todosFavoritos = await _clienteService.GetFavoritos();
            var favoritoExistente = todosFavoritos.FirstOrDefault(f => f.ProdutoId == produtoId && f.UsuarioId == usuarioId);
            if (favoritoExistente != null) return Redirect(Request.Headers["Referer"].ToString());

            var novoFavorito = new ItensFavoritos
            {
                ItemFavoritoId = Guid.NewGuid(),
                ProdutoId = produtoId,
                UsuarioId = usuarioId
            };
            await _clienteService.AdicionarFavorito(novoFavorito);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> RemoverFavorito(Guid itemFavoritoId)
        {
            await _clienteService.RemoverFavorito(itemFavoritoId);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // ================= CARRINHO =================
        public async Task<IActionResult> Carrinho()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var itens = (await _clienteService.GetCarrinho())
                        .Where(c => c.UsuarioId == usuarioId)
                        .ToList();

            ViewBag.AdminService = _adminService;
            ViewBag.Env = _env;

            return View(itens);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarAoCarrinho(Guid produtoId, int quantidade = 1)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var itens = await _clienteService.GetCarrinho();
            var existente = itens.FirstOrDefault(c => c.UsuarioId == usuarioId && c.ProdutoId == produtoId);

            if (existente != null)
            {
                existente.Quantidade += quantidade;
                await _clienteService.EditarItemCarrinho(existente.ItemCarrinhoId, existente);
            }
            else
            {
                var novo = new ItemCarrinhoDTO
                {
                    ItemCarrinhoId = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    ProdutoId = produtoId,
                    Quantidade = quantidade
                };
                await _clienteService.AdicionarItemCarrinho(novo);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarQuantidade(Guid itemCarrinhoId, int quantidade)
        {
            var item = await _clienteService.GetItemCarrinho(itemCarrinhoId);
            if (item == null) return NotFound();

            item.Quantidade = quantidade;
            await _clienteService.EditarItemCarrinho(itemCarrinhoId, item);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> RemoverDoCarrinho(Guid itemCarrinhoId)
        {
            await _clienteService.RemoverItemCarrinho(itemCarrinhoId);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        // ================= PEDIDOS =================
        public async Task<IActionResult> FinalizarPedido()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var enderecos = await _clienteService.GetEnderecosDoUsuarioAsync(usuarioId);
            return View(enderecos);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarPedido(Guid enderecoId, MetodosPagamento metodoPagamento)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToAction("Login", "Conta");

            var carrinho = (await _clienteService.GetCarrinho())
                           .Where(c => c.UsuarioId == usuarioId)
                           .ToList();

            if (!carrinho.Any())
                return BadRequest("Seu carrinho está vazio.");

            // Cria o pedido incluindo o endereço
            var pedidoDto = new PedidoDTO
            {
                PedidoId = Guid.NewGuid(),
                UserId = usuarioId,
                Data = DateTime.UtcNow
            };

            var pedidoCriado = await _clienteService.CriarPedidoAsync(pedidoDto);
            if (!pedidoCriado)
                return BadRequest("Não foi possível criar o pedido.");

            decimal total = 0;

            foreach (var item in carrinho)
            {
                var produto = await _adminService.GetProdutoById(item.ProdutoId);
                if (produto == null)
                    continue;

                var itemDto = new ItemPedidoDTO
                {
                    ItemPedidoId = Guid.NewGuid(),
                    PedidoId = pedidoDto.PedidoId,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade
                };

                var itemCriado = await _clienteService.CriarItemAsync(itemDto);
                if (!itemCriado)
                    return BadRequest($"Não foi possível adicionar o item {produto.Nome} ao pedido.");

                total += (produto.Promocao > 0 ? produto.Promocao : produto.Vista) * item.Quantidade;
            }

            var novaCompraId = Guid.NewGuid();
            var compraDto = new ComprasDTO
            {
                ComprasId = novaCompraId,
                PedidoId = pedidoDto.PedidoId,
                EnderecoId = enderecoId,
                Metodo = metodoPagamento,
                ValorPago = total,
                DataPagamento = DateTime.UtcNow,
                Status = StatusPagamento.Aprovado
            };

            var pagamentoRegistrado = await _clienteService.RegistrarPagamentoAsync(compraDto);
            if (!pagamentoRegistrado)
                return BadRequest("Não foi possível registrar o pagamento.");

            // Limpa o carrinho
            foreach (var item in carrinho)
                await _clienteService.RemoverItemAsync(item.ItemCarrinhoId);

            return RedirectToPage("/Account/Compras/Details", new { area = "Identity", id = novaCompraId });
        }

        public async Task<IActionResult> PedidoConcluido(Guid pedidoId)
        {
            var pedido = await _clienteService.GetPedidoAsync(pedidoId);
            if (pedido == null) return NotFound();

            var itens = await _clienteService.GetItensPedidoAsync(pedidoId);
            var pagamento = (await _clienteService.GetPagamentosDoPedidoAsync(pedidoId)).FirstOrDefault();

            var model = new PedidoViewModel
            {
                Pedido = pedido,
                Itens = itens,
                Pagamento = pagamento
            };

            return View(model);
        }
    }

    public class PedidoViewModel
    {
        public PedidoDTO Pedido { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; } = new();
        public ComprasDTO Pagamento { get; set; }
    }
}