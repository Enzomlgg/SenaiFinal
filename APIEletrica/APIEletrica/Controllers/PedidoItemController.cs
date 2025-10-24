using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoItemController : ControllerBase
    {
        private static List<PedidoItem> _itens = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_itens);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var item = _itens.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound("Item do pedido não encontrado.");

            return Ok(item);
        }

        [HttpGet("pedido/{idPedido}")]
        public IActionResult GetByPedido(long idPedido)
        {
            var itensPedido = _itens.Where(i => i.IdPedido == idPedido).ToList();
            if (!itensPedido.Any())
                return NotFound("Nenhum item encontrado para este pedido.");

            return Ok(itensPedido);
        }

        [HttpPost]
        public IActionResult Post(PedidoItem item)
        {
            if (_itens.Count == 0)
                item.Id = 1;
            else
                item.Id = _itens.Max(i => i.Id) + 1;

            _itens.Add(item);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, PedidoItem itemAtualizado)
        {
            var item = _itens.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound("Item do pedido não encontrado.");

            item.IdPedido = itemAtualizado.IdPedido;
            item.IdProduto = itemAtualizado.IdProduto;
            item.Quantidade = itemAtualizado.Quantidade;
            item.ValorUnitario = itemAtualizado.ValorUnitario;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var item = _itens.FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound("Item do pedido não encontrado.");

            _itens.Remove(item);
            return NoContent();
        }
    }
}
