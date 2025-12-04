using BarraByTechAPI.Data;
using BarraByTechAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarraByTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecosClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnderecosClienteController(AppDbContext context)
        {
            _context = context;
        }

        /* --------------------------------------------------------------
         *      GET: Endereços por usuário
         * -------------------------------------------------------------- */
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<EnderecoClienteDTO>>> GetByUser(Guid userId)
        {
            var enderecos = await _context.EnderecosCliente
                .Where(e => e.UserId == userId)
                .Select(e => new EnderecoClienteDTO
                {
                    EnderecoId = e.EnderecoId,
                    UserId = e.UserId,
                    Cep = e.Cep,
                    Endereco = e.Endereco,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Complemento = e.Complemento
                })
                .ToListAsync();

            return Ok(enderecos);
        }

        /* --------------------------------------------------------------
         *      GET: Endereço por ID
         * -------------------------------------------------------------- */
        [HttpGet("{id}")]
        public async Task<ActionResult<EnderecoClienteDTO>> GetEnderecoCliente(Guid id)
        {
            var e = await _context.EnderecosCliente
                .FirstOrDefaultAsync(x => x.EnderecoId == id);

            if (e == null)
                return NotFound();

            var dto = new EnderecoClienteDTO
            {
                EnderecoId = e.EnderecoId,
                UserId = e.UserId,
                Cep = e.Cep,
                Endereco = e.Endereco,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Complemento = e.Complemento
            };

            return Ok(dto);
        }

        /* --------------------------------------------------------------
         *      POST: Criar endereço
         * -------------------------------------------------------------- */
        [HttpPost]
        public async Task<ActionResult> PostEnderecoCliente(EnderecoClienteDTO dto)
        {
            var model = new EnderecoCliente
            {
                EnderecoId = Guid.NewGuid(),
                UserId = dto.UserId,
                Cep = dto.Cep,
                Endereco = dto.Endereco,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Complemento = dto.Complemento
            };

            _context.EnderecosCliente.Add(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /* --------------------------------------------------------------
         *      PUT: Atualizar endereço
         * -------------------------------------------------------------- */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnderecoCliente(Guid id, EnderecoClienteDTO dto)
        {
            var model = await _context.EnderecosCliente.FindAsync(id);

            if (model == null)
                return NotFound();

            model.Cep = dto.Cep;
            model.Endereco = dto.Endereco;
            model.Cidade = dto.Cidade;
            model.Estado = dto.Estado;
            model.Complemento = dto.Complemento;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /* --------------------------------------------------------------
         *      DELETE: Remover endereço DO USUÁRIO
         * -------------------------------------------------------------- */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnderecoCliente(Guid id, [FromQuery] Guid userId)
        {
            var model = await _context.EnderecosCliente
                .FirstOrDefaultAsync(e => e.EnderecoId == id && e.UserId == userId);

            if (model == null)
                return NotFound();

            _context.EnderecosCliente.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
