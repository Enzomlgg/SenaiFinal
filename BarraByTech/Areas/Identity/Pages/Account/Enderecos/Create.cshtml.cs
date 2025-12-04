using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BarraByTech.Services;
using System.Security.Claims;
using BarraByTech.Models.DTO;

namespace BarraByTech.Areas.Identity.Pages.Account.Enderecos
{
    public class CreateModel : PageModel
    {
        private readonly ClienteService _clienteService;

        public CreateModel(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [BindProperty]
        public EnderecoClienteDTO Endereco { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid usuarioId))
                return RedirectToPage("/Account/Login");

            Endereco.UserId = usuarioId;

            var criado = await _clienteService.CreateEnderecoAsync(Endereco);

            if (!criado)
            {
                ModelState.AddModelError("", "Não foi possível criar o endereço.");
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
