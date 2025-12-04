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
    public class TiposMarcasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposMarcasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposMarcas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TiposMarca>>> GetTiposMarcas()
        {
            return await _context.TiposMarcas.ToListAsync();
        }

        // GET: api/TiposMarcas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TiposMarca>> GetTiposMarca(Guid id)
        {
            var tiposMarca = await _context.TiposMarcas.FindAsync(id);

            if (tiposMarca == null)
            {
                return NotFound();
            }

            return tiposMarca;
        }

        // PUT: api/TiposMarcas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTiposMarca(Guid id, TiposMarca tiposMarca)
        {
            if (id != tiposMarca.TipoMarcaId)
            {
                return BadRequest();
            }

            _context.Entry(tiposMarca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TiposMarcaExists(id))
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

        // POST: api/TiposMarcas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TiposMarca>> PostTiposMarca(TiposMarca tiposMarca)
        {
            _context.TiposMarcas.Add(tiposMarca);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTiposMarca", new { id = tiposMarca.TipoMarcaId }, tiposMarca);
        }

        // DELETE: api/TiposMarcas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTiposMarca(Guid id)
        {
            var tiposMarca = await _context.TiposMarcas.FindAsync(id);
            if (tiposMarca == null)
            {
                return NotFound();
            }

            _context.TiposMarcas.Remove(tiposMarca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("PorMarca")]
        public async Task<ActionResult<IEnumerable<TiposMarca>>> GetTiposPorMarca([FromQuery] Guid marcaId)
        {
            if (marcaId == Guid.Empty)
            {
                return Ok(new List<TiposMarca>());
            }

            var tiposMarca = await _context.TiposMarcas
                .AsNoTracking()
                .Where(t => t.MarcaId == marcaId)
                .ToListAsync();

            return Ok(tiposMarca);
        }

        private bool TiposMarcaExists(Guid id)
        {
            return _context.TiposMarcas.Any(e => e.TipoMarcaId == id);
        }
    }
}
