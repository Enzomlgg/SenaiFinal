using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarraByTechAPI.Data;
using BarraByTechAPI.Models;

namespace BarraByTechAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        // IWebHostEnvironment (e a injeção dele) não é mais necessário.
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // --- GETS (Inalterados) ---
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos
                .Include(e => e.Categoria)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(Guid id)
        {
            var produto = await _context.Produtos
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpGet("GetPorCategoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPorCategoria(Guid categoriaId)
        {
            var produtos = await _context.Produtos
                .AsNoTracking()
                .Where(p => p.CategoriaId == categoriaId)
                .Include(p => p.Categoria)
                .ToListAsync();

            return Ok(produtos);
        }

        // --- POST (Recebe JSON diretamente no corpo) ---
        [HttpPost]
        // Recebe o objeto Produto do Body, não mais de [FromForm]
        public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
        {
            // O ASP.NET Core agora faz a desserialização JSON automaticamente.
            if (produto == null)
            {
                return BadRequest("Dados do produto ausentes no corpo da requisição.");
            }

            if (produto.CategoriaId == Guid.Empty)
            {
                return BadRequest("ID da Categoria é obrigatório.");
            }
            if (produto.MarcaId == Guid.Empty)
            {
                return BadRequest("ID da Marca é obrigatório.");
            }
            if (produto.TipoMarcaId == Guid.Empty)
            {
                return BadRequest("ID do Tipo de Marca é obrigatório.");
            }

            produto.ProdutoId = Guid.NewGuid();
            produto.DataCriacao = DateTime.UtcNow;

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.ProdutoId }, produto);
        }

        // --- PUT (Recebe JSON diretamente no corpo) ---
        [HttpPut("{id}")]
        // Recebe o objeto Produto do Body, não mais de [FromForm]
        public async Task<IActionResult> PutProduto(Guid id, [FromBody] Produto produtoAtualizado)
        {
            if (produtoAtualizado == null)
            {
                return BadRequest("Dados do produto ausentes no corpo da requisição para edição.");
            }

            if (id != produtoAtualizado.ProdutoId || produtoAtualizado.ProdutoId == Guid.Empty)
            {
                return BadRequest("ID do produto inválido ou incompatível.");
            }

            var produtoOriginal = await _context.Produtos.FindAsync(id);
            if (produtoOriginal == null) return NotFound();

            // Atualiza os valores do produto existente com os novos valores.
            _context.Entry(produtoOriginal).CurrentValues.SetValues(produtoAtualizado);

            _context.Entry(produtoOriginal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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

        // --- DELETE (Simplificado) ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

           
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Produto>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Ok(new List<Produto>());
            }

            string termoDeBusca = q.Trim().ToLower();

            var produtos = await _context.Produtos
                .AsNoTracking()
                .Where(p => p.Nome.ToLower().Contains(termoDeBusca))
                .Take(5)
                .ToListAsync();

            return Ok(produtos);
        }

        private bool ProdutoExists(Guid id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }
    }
}