using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using BarraByTech.Services;
using System.Security.Claims;
using BarraByTech.Models.DTO;

namespace BarraByTech.Areas.Identity.Pages.Account.Enderecos
{
    public class IndexModel : PageModel
    {
        private readonly ClienteService _clienteService;

        public IndexModel(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public List<EnderecoClienteDTO> Enderecos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
            {
                return RedirectToPage("/Account/Login");
            }

            Enderecos = await _clienteService.GetEnderecosDoUsuarioAsync(usuarioId);
            return Page();
        }
    }
}
