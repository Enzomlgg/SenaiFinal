using BarraByTech.Models;
using Microsoft.AspNetCore.Mvc;
using BarraByTech.Services;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarraByTech.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ClienteService _clienteService;

        public CategoriasController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Detalhes(Guid id)
        {
            var produtosDto = await _clienteService.GetProdutoCategoria(id);
            var categoriaDto = await _clienteService.GetNomeCategoria(id.ToString());
            ViewBag.NomeCategoria = categoriaDto?.CategoriaNome;

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid usuarioId = Guid.Empty;
            if (User.Identity.IsAuthenticated && Guid.TryParse(userIdString, out Guid idUsuario))
            {
                usuarioId = idUsuario;
            }

            Dictionary<Guid, Guid> favoritosDoUsuarioMap = new Dictionary<Guid, Guid>();
            if (usuarioId != Guid.Empty)
            {
                var todosFavoritos = await _clienteService.GetFavoritos();

                favoritosDoUsuarioMap = todosFavoritos
                    .Where(f => f.UsuarioId == usuarioId)
                    .ToDictionary(f => f.ProdutoId, f => f.ItemFavoritoId);
            }

            ViewBag.ProdutosFavoritadosMap = favoritosDoUsuarioMap;

            return View(produtosDto);
        }
    }
}