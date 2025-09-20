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
    public class CobradorController(ILogger<CobradorController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessCobrador _cobrador = new BusinessCobrador(context);
        private readonly ILogger<CobradorController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Cobrador> cobradores;
            try
            {
                cobradores = await _cobrador.GetAll();
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
                return BadRequest("Id incorrecto.");
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
            if (nombre == null || aPaterno == null || aMaterno == null || status == null || usuario == null)
            {
                return BadRequest("Datos incompletos. Verifique.");
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
                return BadRequest("Id incorrecto.");
            }

            try
            {
                var result = await _cobrador.Delete(id);
                return Ok("Cobrador eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cobrador no encontrado"))
                    return StatusCode(500, "El Cobrador que intenta eliminar no existe en la base de datos.");

                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string nombre, string aPaterno, string aMaterno,
            string telefono, string email, string status, string usuario)
        {
            if (nombre == null || aPaterno == null || aMaterno == null || status == null || usuario == null)
            {
                return BadRequest("Datos incompletos. Verifique.");
            }

            try
            {
                var result = await _cobrador.Update(id, nombre, aPaterno, aMaterno, telefono,
                    email, status, usuario);
                return Ok("Cobrador actualizado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cobrador no encontrado"))
                    return StatusCode(500, "El cobrador que intenta actualizar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
