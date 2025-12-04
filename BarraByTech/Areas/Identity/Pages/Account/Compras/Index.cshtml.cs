using BarraByTech.Models.DTO;
using BarraByTech.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BarraByTech.Areas.Identity.Pages.Account.Compras
{
    public class IndexModel : PageModel
    {
        private readonly ClienteService _clientService;

        public IndexModel(ClienteService clientService)
        {
            _clientService = clientService;
        }

        public List<ComprasDTO> Compras { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
            {
                Compras.Clear();
                return;
            }

            Compras = await _clientService.GetComprasDoUsuarioAsync(userId);
        }
    }
}
