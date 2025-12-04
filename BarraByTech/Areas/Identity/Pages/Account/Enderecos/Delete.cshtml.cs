using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BarraByTech.Services;
using System.Security.Claims;
using BarraByTech.Models.DTO;

namespace BarraByTech.Areas.Identity.Pages.Account.Enderecos
{
    public class DeleteModel : PageModel
    {
        private readonly ClienteService _clienteService;

        public DeleteModel(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [BindProperty]
        public EnderecoClienteDTO Endereco { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var endereco = await _clienteService.GetEnderecoByIdAsync(id);

            if (endereco == null)
                return NotFound();

            Endereco = endereco;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var endereco = await _clienteService.GetEnderecoByIdAsync(id);

            if (endereco == null)
                return NotFound();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToPage("/Account/Login");

            var deleted = await _clienteService.DeleteEnderecoAsync(id, usuarioId);

            if (!deleted)
            {
                ModelState.AddModelError("", "Não foi possível deletar o endereço.");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
