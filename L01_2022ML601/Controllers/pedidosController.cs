using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly restauranteDBContext _context;
        public pedidosController(restauranteDBContext context)
        {
            _context = context;


        }
        // Consultar todos los pedidos
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get() 
        { 
            var pedidos =(from p in _context.Pedidos
                          join c in _context.Clientes on p.clienteId equals c.clienteId
                          join m in _context.Motoristas on p.motoristaId equals m.motoristaId
                          join pl in _context.Platos on p.platoId equals pl.platoId
                          select new 
                          { 
                            p.pedidoId,
                            Cliente = c.nombreCliente,
                            Motoristas = m.nombreMotorista,
                            Plato = pl.nombrePlato,
                            p.cantidad,
                            p.precio,
                            Total = p.cantidad * p.precio
                          }).ToList();

            if (pedidos.Count == 0) 
            {
                return NotFound();
            }
            return Ok(pedidos);
        }
        //agregar un pedido
        [HttpPost]
        [Route("Add")]
        public IActionResult Agregar([FromBody] Pedidos pedido)
        {
            try
            {
                _context.Pedidos.Add(pedido);
                _context.SaveChanges();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //actualizar un pedido
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult actualizar(int id, [FromBody] Pedidos pedido) 
        {
            var extpedido = _context.Pedidos.Find(id);
            if (extpedido == null) 
            {
                return NotFound();
            }
            extpedido.motoristaId = pedido.motoristaId;
            extpedido.clienteId = pedido.clienteId;
            extpedido.platoId = pedido.platoId;
            extpedido.cantidad = pedido.cantidad;
            extpedido.precio = pedido.precio;

            _context.SaveChanges();
            return Ok(pedido);

        }
        // eliminar un pedido
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult borrar(int id) 
        {
            var pedido = _context.Pedidos.Find(id);
            if(pedido == null) 
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();
            return Ok(pedido);
        }
        // flitrado por cliente
        [HttpGet]
        [Route("GetByCliente/{clienteId}")]
        public IActionResult filtradoPorCliente(int clienteId) 
        { 
            var pedidos = (from p in _context.Pedidos
                          where p.clienteId == clienteId
                          join c in _context.Clientes on p.clienteId equals c.clienteId
                          join m in _context.Motoristas on p.motoristaId equals m.motoristaId
                          join pl in _context.Platos on p.platoId equals pl.platoId
                          select new 
                          {
                              p.pedidoId,
                              Cliente = c.nombreCliente,
                              Motorista = m.nombreMotorista,
                              Plato = pl.nombrePlato,
                              p.cantidad,
                              p.precio,
                              Total = p.cantidad * p.precio

                          }).ToList();
            if (pedidos.Count == 0) 
            {
                return NotFound();
            }
            return Ok(pedidos);
        }
        // filtrado por motorista
        [HttpGet]
        [Route("GetByMotorista/{motoristaId}")]
        public IActionResult filtradoPorMotorista(int motoristaId) 
        {
            var pedidos = (from p in _context.Pedidos
                           where p.motoristaId == motoristaId
                           join c in _context.Clientes on p.clienteId equals c.clienteId
                           join m in _context.Motoristas on p.motoristaId equals m.motoristaId
                           join pl in _context.Platos on p.platoId equals pl.platoId
                           select new
                           {
                               p.pedidoId,
                               Cliente = c.nombreCliente,
                               Motorista = m.nombreMotorista,
                               Plato = pl.nombrePlato,
                               p.cantidad,
                               p.precio,
                               Total = p.cantidad * p.precio
                           }).ToList();
            if (pedidos.Count == 0)
            {
                return NotFound();
            }
            return Ok(pedidos);
        }
    }
}
