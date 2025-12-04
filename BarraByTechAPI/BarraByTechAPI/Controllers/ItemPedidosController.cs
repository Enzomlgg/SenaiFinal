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
    public class ItemPedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemPedidosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ItemPedidoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemPedido>>> GetItensPedidos()
        {
            return await _context.ItensPedidos
                .Include(v => v.Pedido)
                    .ThenInclude(v => v.Cliente)
                .Include(p => p.Produto)
                    .ThenInclude(v => v.Categoria)
                .ToListAsync();
        }

        // GET: api/ItemPedidoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPedido>> GetItemPedido(Guid id)
        {
            var itemPedido = await _context.ItensPedidos
                .Include(v => v.Pedido)
                    .ThenInclude(v => v.Cliente)
                .Include(p => p.Produto)
                    .ThenInclude(v => v.Categoria)
                .FirstOrDefaultAsync(e => e.ItemPedidoId == id);

            if (itemPedido == null)
            {
                return NotFound();
            }

            return itemPedido;
        }

        [HttpGet("PedidoId/{pedidoId}")]
        public async Task<ActionResult<IEnumerable<ItemPedidoDTO>>> GetPorPedido(Guid pedidoId) // Mudança aqui
        {
            var itensPedido = await _context.ItensPedidos
                .AsNoTracking()
                .Where(i => i.PedidoId == pedidoId)
                .Select(i => new ItemPedidoDTO // 
                {
                    ItemPedidoId = i.ItemPedidoId,
                    PedidoId = i.PedidoId,
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                })
                .ToListAsync();

            if (!itensPedido.Any())
                return NotFound();

            return Ok(itensPedido); 
        }

        // PUT: api/ItemPedidoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemPedido(Guid id, ItemPedido itemPedido)
        {
            if (id != itemPedido.ItemPedidoId)
            {
                return BadRequest();
            }

            _context.Entry(itemPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemPedidoExists(id))
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

        // POST: api/ItemPedidoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItemPedido>> PostItemPedido(ItemPedido itemPedido)
        {
            _context.ItensPedidos.Add(itemPedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemPedido", new { id = itemPedido.ItemPedidoId }, itemPedido);
        }

        // DELETE: api/ItemPedidoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemPedido(Guid id)
        {
            var itemPedido = await _context.ItensPedidos.FindAsync(id);
            if (itemPedido == null)
            {
                return NotFound();
            }

            _context.ItensPedidos.Remove(itemPedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemPedidoExists(Guid id)
        {
            return _context.ItensPedidos.Any(e => e.ItemPedidoId == id);
        }
    }
}
