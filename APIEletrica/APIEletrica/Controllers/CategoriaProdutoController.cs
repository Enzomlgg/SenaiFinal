using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaProdutoController : ControllerBase
    {
        private static List<CategoriaProduto> _subcategorias = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_subcategorias);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var subcategoria = _subcategorias.FirstOrDefault(c => c.Id == id);
            if (subcategoria == null)
                return NotFound("Subcategoria não encontrada.");

            return Ok(subcategoria);
        }

        [HttpGet("categoria/{idCategoria}")]
        public IActionResult GetByCategoria(long idCategoria)
        {
            var lista = _subcategorias.Where(c => c.IdCategoria == idCategoria).ToList();
            if (!lista.Any())
                return NotFound("Nenhuma subcategoria para esta categoria.");

            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Post(CategoriaProduto subcategoria)
        {
            // Simula ID incremental
            if (_subcategorias.Count == 0)
                subcategoria.Id = 1;
            else
                subcategoria.Id = _subcategorias.Max(c => c.Id) + 1;

            _subcategorias.Add(subcategoria);
            return Ok(subcategoria);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, CategoriaProduto subcategoriaAtualizada)
        {
            var subcategoria = _subcategorias.FirstOrDefault(c => c.Id == id);
            if (subcategoria == null)
                return NotFound("Subcategoria não encontrada.");

            subcategoria.Nome = subcategoriaAtualizada.Nome;
            subcategoria.IdCategoria = subcategoriaAtualizada.IdCategoria;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var subcategoria = _subcategorias.FirstOrDefault(c => c.Id == id);
            if (subcategoria == null)
                return NotFound("Subcategoria não encontrada.");

            _subcategorias.Remove(subcategoria);
            return NoContent();
        }
    }
}