using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValoresProdutoController : ControllerBase
    {
        private static List<ValoresProduto> _valores = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_valores);
        }

        [HttpGet("{idProduto}")]
        public IActionResult GetByProduto(long idProduto)
        {
            var valores = _valores.FirstOrDefault(v => v.IdProduto == idProduto);
            if (valores == null)
                return NotFound("Valores do produto não encontrados.");

            return Ok(valores);
        }

        [HttpPost]
        public IActionResult Post(ValoresProduto valoresProduto)
        {
            var existe = _valores.Any(v => v.IdProduto == valoresProduto.IdProduto);
            if (existe)
                return Conflict("Valores para este produto já existem.");

            _valores.Add(valoresProduto);
            return Ok(valoresProduto);
        }

        [HttpPut("{idProduto}")]
        public IActionResult Update(long idProduto, ValoresProduto valoresAtualizados)
        {
            var valores = _valores.FirstOrDefault(v => v.IdProduto == idProduto);
            if (valores == null)
                return NotFound("Valores do produto não encontrados.");

            valores.ValorDesconto = valoresAtualizados.ValorDesconto;
            valores.ValorAVista = valoresAtualizados.ValorAVista;
            valores.ValorCredito = valoresAtualizados.ValorCredito;
            valores.ValorEstoque = valoresAtualizados.ValorEstoque;

            return NoContent();
        }

        [HttpDelete("{idProduto}")]
        public IActionResult Delete(long idProduto)
        {
            var valores = _valores.FirstOrDefault(v => v.IdProduto == idProduto);
            if (valores == null)
                return NotFound("Valores do produto não encontrados.");

            _valores.Remove(valores);
            return NoContent();
        }
    }
}
