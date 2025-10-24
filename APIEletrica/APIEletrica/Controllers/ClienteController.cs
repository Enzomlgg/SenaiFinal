using APIEletrica.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIEletrica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private static List<Cliente> _clientes = new();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Post(Cliente c)
        {
            // Simula ID incremental
            if (_clientes.Count == 0)
                c.Id = 1;
            else
                c.Id = _clientes.Max(x => x.Id) + 1;

            c.DataCriacao = DateTime.Now;

            _clientes.Add(c);
            return Ok(c);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, Cliente clienteAtualizado)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound();

            cliente.Nome = clienteAtualizado.Nome;
            cliente.CpfCnpj = clienteAtualizado.CpfCnpj;
            cliente.Tipo = clienteAtualizado.Tipo;
            cliente.Email = clienteAtualizado.Email;
            cliente.Telefone = clienteAtualizado.Telefone;
            cliente.DataNascimento = clienteAtualizado.DataNascimento;
            cliente.Logradouro = clienteAtualizado.Logradouro;
            cliente.Numero = clienteAtualizado.Numero;
            cliente.Complemento = clienteAtualizado.Complemento;
            cliente.Bairro = clienteAtualizado.Bairro;
            cliente.Cep = clienteAtualizado.Cep;
            cliente.Municipio = clienteAtualizado.Municipio;
            cliente.Estado = clienteAtualizado.Estado;
            cliente.Pais = clienteAtualizado.Pais;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var cliente = _clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound();

            _clientes.Remove(cliente);
            return NoContent();
        }
    }
}