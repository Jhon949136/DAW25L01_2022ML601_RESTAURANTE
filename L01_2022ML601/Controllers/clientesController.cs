using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clientesController : ControllerBase
    {
        private readonly restauranteDBContext _context;
        public clientesController(restauranteDBContext context)
        {
            _context = context;


        }
        //consltar todos los clientes
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoClientes = (from c in _context.Clientes
                                   select new
                                   {
                                       c.clienteId,
                                       c.nombreCliente,
                                       c.direccion
                                   }).ToList();

            if (listadoClientes.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoClientes);
        }
        //consultar cliente por id
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            Clientes? cliente = (from c in _context.Clientes
                                where c.clienteId == id
                                select c).FirstOrDefault();

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }
        //Consulat cliente por direccion
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult BuscarPorDireccion(string filtro)
        {
            var clientes = (from c in _context.Clientes
                            where c.direccion.Contains(filtro)
                            select new
                            {
                                c.clienteId,
                                c.nombreCliente,
                                c.direccion
                            }).ToList();

            if (clientes.Count == 0)
            {
                return NotFound();
            }

            return Ok(clientes);
        }
        //Agregar cliente
        [HttpPost]
        [Route("Add")]
        public IActionResult AgregarCliente([FromBody] Clientes cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Actualizar cliente
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCliente(int id, [FromBody] Clientes clienteModificar)
        {
            Clientes? clienteActual = (from c in _context.Clientes
                                      where c.clienteId == id
                                      select c).FirstOrDefault();

            if (clienteActual == null)
            {
                return NotFound();
            }

            clienteActual.nombreCliente = clienteModificar.nombreCliente;
            clienteActual.direccion = clienteModificar.direccion;

            _context.Entry(clienteActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(clienteModificar);
        }
        //Elminar cliente
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarCliente(int id)
        {
            Clientes? cliente = (from c in _context.Clientes
                                where c.clienteId == id
                                select c).FirstOrDefault();

            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Attach(cliente);
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return Ok(cliente);
        }
        // TOP N de los clientes que mas ventas en pedidos tienen
        [HttpGet]
        [Route("TopNClientes/{n}")]
        public IActionResult ObtenerTopNDeClientes(int n)
        {
            var topNClientes = (from p in _context.Pedidos
                                group p by p.clienteId into groupedPedidos
                                join c in _context.Clientes on groupedPedidos.Key equals c.clienteId
                                select new
                                {
                                    c.clienteId,
                                    c.nombreCliente,
                                    TotalVentas = groupedPedidos.Sum(p => p.cantidad * p.precio)
                                })
                                .OrderByDescending(x => x.TotalVentas)
                                .Take(n)
                                .ToList();

            if (topNClientes.Count == 0)
            {
                return NotFound();
            }

            return Ok(topNClientes);
        }
    }
}
