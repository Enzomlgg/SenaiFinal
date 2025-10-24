using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private static List<Pedido> _pedidos = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_pedidos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            return Ok(pedido);
        }

        [HttpPost]
        public IActionResult Post(Pedido pedido)
        {
            if (_pedidos.Count == 0)
                pedido.Id = 1;
            else
                pedido.Id = _pedidos.Max(p => p.Id) + 1;

            _pedidos.Add(pedido);
            return Ok(pedido);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, Pedido pedidoAtualizado)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            pedido.IdCliente = pedidoAtualizado.IdCliente;
            pedido.DataHora = pedidoAtualizado.DataHora;
            pedido.Status = pedidoAtualizado.Status;
            pedido.DataEntregaPrevista = pedidoAtualizado.DataEntregaPrevista;
            pedido.Observacoes = pedidoAtualizado.Observacoes;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido == null)
                return NotFound("Pedido não encontrado.");

            _pedidos.Remove(pedido);
            return NoContent();
        }
    }
}
