using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private static List<Categoria> _categorias = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categorias);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var categoria = _categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public IActionResult Post(Categoria categoria)
        {
            // Simula ID incremental
            if (_categorias.Count == 0)
                categoria.Id = 1;
            else
                categoria.Id = _categorias.Max(c => c.Id) + 1;

            _categorias.Add(categoria);
            return Ok(categoria);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, Categoria categoriaAtualizada)
        {
            var categoria = _categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
                return NotFound();

            categoria.Nome = categoriaAtualizada.Nome;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var categoria = _categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
                return NotFound();

            _categorias.Remove(categoria);
            return NoContent();
        }
    }
}