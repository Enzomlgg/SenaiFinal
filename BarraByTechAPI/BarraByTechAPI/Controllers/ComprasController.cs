using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarraByTechAPI.Data;
using BarraByTechAPI.Models;

namespace BarraByTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComprasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Compras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compras>>> GetCompras()
        {
            return await _context.Compras
                .Include(x => x.Pedido)
                    .ThenInclude(x => x.Cliente)
                .ToListAsync();
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compras>> GetCompras(Guid id)
        {
            var compras = await _context.Compras
                .Include(x => x.Pedido)
                    .ThenInclude(x => x.Cliente)
                .FirstOrDefaultAsync(e => e.ComprasId == id);

            if (compras == null)
            {
                return NotFound();
            }

            return compras;
        }

        // PUT: api/Compras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompras(Guid id, Compras compras)
        {
            if (id != compras.ComprasId)
            {
                return BadRequest();
            }

            _context.Entry(compras).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComprasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("PorUsuario/{userId}")]
        public async Task<ActionResult<IEnumerable<Compras>>> GetComprasDoUsuario(Guid userId)
        {
            var compras = await _context.Compras
                .Include(x => x.Pedido)
                    .ThenInclude(x => x.Cliente)
                .Where(c => c.Pedido.UserId == userId)
                .ToListAsync();

            if (compras == null || !compras.Any())
                return NotFound();

            return compras;
        }

        // POST: api/Compras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras(Compras compras)
        {
            _context.Compras.Add(compras);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompras", new { id = compras.ComprasId }, compras);
        }

        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompras(Guid id)
        {
            var compras = await _context.Compras.FindAsync(id);
            if (compras == null)
            {
                return NotFound();
            }

            _context.Compras.Remove(compras);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComprasExists(Guid id)
        {
            return _context.Compras.Any(e => e.ComprasId == id);
        }
    }
}
