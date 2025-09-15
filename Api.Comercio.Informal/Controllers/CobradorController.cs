using Api.Business;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Api.Comercio.Informal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CobradorController : ControllerBase
    {
        private readonly ILogger<CobradorController> _logger;
        private readonly BusinessCobrador _cobrador;

        public CobradorController(ILogger<CobradorController> logger, MySQLiteContext context)
        {
            _logger = logger;
            _cobrador = new BusinessCobrador(context);
        }

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Cobrador> cobradores;
            try
            {
                cobradores = await _cobrador.GetAll();

                foreach (Cobrador cobrador in cobradores)
                {
                    cobrador.Fecha_alta = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(cobradores);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id del cobrador es necesario.");
            }

            Cobrador cobrador;

            try
            {
                cobrador = await _cobrador.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(cobrador);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string aPaterno, string aMaterno,
            string telefono, string email, string status, string usuario)
        {
            if (nombre == null)
            {
                return BadRequest("El nombre del cobrador es necesario.");
            }

            if (usuario == null)
            {
                return BadRequest("El usuario es necesario.");
            }

            try
            {
                Cobrador cobrador = new()
                {
                    Nombre = nombre,
                    A_paterno = aPaterno,
                    A_materno = aMaterno,
                    Telefono = telefono,
                    Email = email,
                    Estado = status,
                    Usuario_alta = usuario,
                    Fecha_alta = DateTime.Now
                };

                var result = await _cobrador.Create(cobrador);

                return Ok("Cobrador registrado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id del cobrador es necesario.");
            }
            try
            {
                var result = await _cobrador.Delete(id);
                return Ok("Cobrador eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cobrador no encontrado"))
                    return StatusCode(500, "La Cobrador que intenta eliminar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string nombre, string aPaterno, string aMaterno,
            string telefono, string email, string status, string usuario)
        {
            if (nombre == null || aPaterno == null || aMaterno == null || telefono == null || email == null ||
                status == null)
            {
                return BadRequest("Dato del cobrador incompletos, verifique.");
            }

            if (id <= 0)
            {
                return BadRequest("El id del trabajador no es válido, verifique.");
            }

            Cobrador cobrador;

            try
            {
                var result = await _cobrador.Update(id, nombre, aPaterno, aMaterno, telefono,
                    email, status, usuario);
                return Ok("Categoria actualizada correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Categoria no encontrada"))
                    return StatusCode(500, "La categoría que intenta actualizar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
