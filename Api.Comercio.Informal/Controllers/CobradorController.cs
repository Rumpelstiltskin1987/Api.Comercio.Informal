using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
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

        [Route("Search")]
        [HttpGet]
        public async Task<IActionResult> Search(string? nombre, string? aPaterno, string? aMaterno,
            string? telefono, string? email, string? estado)
        {
            try
            {
                var cobradores = await _cobrador.Search(nombre, aPaterno, aMaterno,
                    telefono, email, estado);

                if (!cobradores.Any())
                {
                    return NotFound("No se encontraron resultados con los criterios proporcionados.");
                }

                return Ok(cobradores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string aPaterno, string aMaterno,
            string? telefono, string? email, string usuario)
        {
            try
            {
                await _cobrador.Create(nombre,aPaterno, aMaterno,
                    telefono, email, usuario);
                return Ok("Cobrador registrado correctamente");
            }
            catch (Exception ex)
            {
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
                await _cobrador.Update(id, nombre, aPaterno, aMaterno, telefono,
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
                await _cobrador.Delete(id);
                return Ok("Cobrador eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cobrador no encontrado"))
                    return StatusCode(500, "El Cobrador que intenta eliminar no existe en la base de datos.");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
