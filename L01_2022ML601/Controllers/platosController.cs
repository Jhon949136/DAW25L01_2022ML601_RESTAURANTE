using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace L01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteDBContext _context;
        public platosController(restauranteDBContext context)
        {
            _context = context;


        }
        //consultar platos
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get() 
        {
            var listadoplatos = (from p in _context.Platos
                                 select new
                                 {
                                     p.platoId,
                                     p.nombrePlato,
                                     p.precio
                                 }).ToList();
            if (listadoplatos.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoplatos);
        }
        //consultar plato por id
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id) 
        {
            Platos? plato = (from p in _context.Platos
                            where p.platoId == id
                            select p).FirstOrDefault();

            if (plato == null)
            {
                return NotFound();
            }

            return Ok(plato);
        }
        //consultar plato por nombre del plato
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult filtroPorNombre(string filtro)
        {
            var platos = (from p in _context.Platos
                          where p.nombrePlato.Contains(filtro)
                          select new
                          {
                              p.platoId,
                              p.nombrePlato,
                              p.precio
                          }).ToList();

            if (platos.Count == 0)
            {
                return NotFound();
            }

            return Ok(platos);
        }
        //agregar plato
        [HttpPost]
        [Route("Add")]
        public IActionResult AgregarPlato([FromBody] Platos plato)
        {
            try
            {
                _context.Platos.Add(plato);
                _context.SaveChanges();
                return Ok(plato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //actualizar plato
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizarPlato(int id, [FromBody] Platos platoModificar)
        {
            Platos? platoActual = (from p in _context.Platos
                                  where p.platoId == id
                                  select p).FirstOrDefault();

            if (platoActual == null)
            {
                return NotFound();
            }

            platoActual.nombrePlato = platoModificar.nombrePlato;
            platoActual.precio = platoModificar.precio;

            _context.Entry(platoActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(platoModificar);
        }
        //eliminar plato
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult eliminarPlato(int id)
        {
            Platos? plato = (from p in _context.Platos
                            where p.platoId == id
                            select p).FirstOrDefault();

            if (plato == null)
            {
                return NotFound();
            }

            _context.Platos.Attach(plato);
            _context.Platos.Remove(plato);
            _context.SaveChanges();

            return Ok(plato);
        }


    }
}

