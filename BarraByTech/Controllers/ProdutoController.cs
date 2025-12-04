using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarraByTech.Models;
using BarraByTech.Services;
using System.Security.Claims;
using BarraByTech.Models.DTO;

namespace BarraByTech.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IWebHostEnvironment Env;
        private readonly AdminService _adminService;
        private readonly ClienteService _clienteService;

        public ProdutoController(IWebHostEnvironment env, AdminService adminService, ClienteService clienteService)
        {
            Env = env;
            _adminService = adminService;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> DetalheProduto(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            ProdutoDTO produtoDto;
            try
            {
                produtoDto = await _adminService.GetProdutoById(id);
            }
            catch (Exception ex)
            {
                return NotFound($"Não foi possível carregar os detalhes do produto com ID: {id}. Erro: {ex.Message}");
            }

            if (produtoDto == null)
            {
                return NotFound($"O produto com ID {id} não foi encontrado.");
            }

            // LÓGICA DO FAVORITO: Verifica se o produto está na lista de favoritos do usuário
            Guid itemFavoritoId = Guid.Empty;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.Identity.IsAuthenticated && Guid.TryParse(userIdString, out Guid usuarioId))
            {
                // NOTA: É melhor ter um método no Service que busque APENAS o favorito de um ProdutoId e UsuarioId específicos,
                // em vez de carregar todos os favoritos na memória. Ex: GetFavoritoByProdutoAndUser(usuarioId, id)
                var todosFavoritos = await _clienteService.GetFavoritos();
                var favoritoExistente = todosFavoritos
                    .FirstOrDefault(f => f.UsuarioId == usuarioId && f.ProdutoId == id);

                if (favoritoExistente != null)
                {
                    itemFavoritoId = favoritoExistente.ItemFavoritoId;
                }
            }

            // Passa o ItemFavoritoId para a View (Se for Guid.Empty, significa que não está favoritado)
            ViewData["ItemFavoritoId"] = itemFavoritoId;

            var produtoDetalhe = new ProdutoDetalheViewModel(produtoDto);

            try
            {
                var marca = await _clienteService.GetMarca(produtoDetalhe.MarcaId.ToString());
                produtoDetalhe.MarcaNome = marca?.Nome ?? "Marca Desconhecida";

                var tipoMarca = await _clienteService.GetTipoMarca(produtoDetalhe.TipoMarcaId.ToString());
                produtoDetalhe.TipoMarcaNome = tipoMarca?.Nome ?? "Tipo Desconhecido";
            }
            catch (Exception)
            {
                produtoDetalhe.MarcaNome = "Marca (Erro na Busca)";
                produtoDetalhe.TipoMarcaNome = "Tipo (Erro na Busca)";
            }

            string produtoIdString = produtoDetalhe.ProdutoId.ToString();
            string pastaFisica = Path.Combine(Env.WebRootPath, "Imagens", "Produtos", produtoIdString);
            string pastaVirtual = $"/Imagens/Produtos/{produtoIdString}/";

            var imagens = new List<string>();

            if (Directory.Exists(pastaFisica))
            {
                string[] extensoes = { "*.png", "*.jpg", "*.jpeg", "*.webp" };
                foreach (var ext in extensoes)
                {
                    var arquivos = Directory.GetFiles(pastaFisica, ext).OrderBy(f => f);
                    imagens.AddRange(arquivos.Select(Path.GetFileName).ToList());
                }
            }

            produtoDetalhe.ImagensUrl = imagens.Select(f => pastaVirtual + f).ToList();

            if (!produtoDetalhe.ImagensUrl.Any())
            {
                produtoDetalhe.ImagensUrl.Add("/img/placeholder.png");
            }

            return View(produtoDetalhe);
        }
    }
}