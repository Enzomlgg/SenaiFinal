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
    public class ItensFavoritosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItensFavoritosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ItensFavoritos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItensFavoritos>>> GetItensFavoritos()
        {
            return await _context.ItensFavoritos.ToListAsync();
        }

        // GET: api/ItensFavoritos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItensFavoritos>> GetItensFavoritos(Guid id)
        {
            var itensFavoritos = await _context.ItensFavoritos.FindAsync(id);

            if (itensFavoritos == null)
            {
                return NotFound();
            }

            return itensFavoritos;
        }

        // PUT: api/ItensFavoritos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItensFavoritos(Guid id, ItensFavoritos itensFavoritos)
        {
            if (id != itensFavoritos.ItemFavoritoId)
            {
                return BadRequest();
            }

            _context.Entry(itensFavoritos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItensFavoritosExists(id))
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

        // POST: api/ItensFavoritos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItensFavoritos>> PostItensFavoritos(ItensFavoritos itensFavoritos)
        {
            _context.ItensFavoritos.Add(itensFavoritos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItensFavoritos", new { id = itensFavoritos.ItemFavoritoId }, itensFavoritos);
        }

        // DELETE: api/ItensFavoritos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItensFavoritos(Guid id)
        {
            var itensFavoritos = await _context.ItensFavoritos.FindAsync(id);
            if (itensFavoritos == null)
            {
                return NotFound();
            }

            _context.ItensFavoritos.Remove(itensFavoritos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItensFavoritosExists(Guid id)
        {
            return _context.ItensFavoritos.Any(e => e.ItemFavoritoId == id);
        }
    }
}
