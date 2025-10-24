using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private static List<Produto> _produtos = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_produtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Post(Produto produto)
        {
            // Simula ID incremental
            if (_produtos.Count == 0)
                produto.Id = 1;
            else
                produto.Id = _produtos.Max(p => p.Id) + 1;

            produto.DataCriacao = DateTime.Now;
            produto.Ativo = true; // comportamento padrão, igual ao DEFAULT 1 no SQL

            _produtos.Add(produto);
            return Ok(produto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, Produto produtoAtualizado)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return NotFound();

            produto.Nome = produtoAtualizado.Nome;
            produto.Descricao = produtoAtualizado.Descricao;
            produto.Imagens = produtoAtualizado.Imagens;
            produto.Estoque = produtoAtualizado.Estoque;
            produto.EstoqueMinimo = produtoAtualizado.EstoqueMinimo;
            produto.Ativo = produtoAtualizado.Ativo;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
                return NotFound();

            _produtos.Remove(produto);
            return NoContent();
        }
    }
}
