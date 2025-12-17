using Api.Business;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PadronController(ILogger<PadronController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessPadron _padron = new(context);
        private readonly ILogger<PadronController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Padron> padron;

            try
            {
               padron = await _padron.GetAll();

                if (!padron.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(padron);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Padron vendedor;

            try
            {
                vendedor = await _padron.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(vendedor);
        }

        [Route("Search")]
        [HttpGet]
        public async Task<IActionResult> Search(string? nombre, string? aPaterno, string? aMaterno,
            string? curp, string? matricula, int idGremio, string? tipo, string? estado)
        {
            try
            {
                var afiliados = await _padron.Search(nombre, aPaterno,  aMaterno,
            curp, matricula, idGremio, tipo, estado);

                if (!afiliados.Any())
                {
                    return NotFound("No se encontraron resultados con los criterios proporcionados.");
                }

                return Ok(afiliados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DtoPadron request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _padron.Create(
                    request.Nombre,
                    request.APaterno,
                    request.AMaterno,
                    request.Curp,
                    request.Direccion,
                    request.Telefono,
                    request.Email,
                    request.IdGremio,
                    request.Tipo,
                    request.Usuario
                );

                return Ok(new { mensaje = "Contribuyente registrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string nombre, string a_paterno, string a_materno, string curp,
            string direccion, string telefono, string email, string matricula, string matricula_anterior, int id_gremio, 
            string status, string usuario)
        {
            try
            {                
                await _padron.Update(id, nombre, a_paterno, a_materno, curp,
                    direccion, telefono, email, matricula, matricula_anterior,
                    id_gremio, status, usuario);
                return Ok("Contribuyente actualizado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Padrón no encontrado"))
                    return StatusCode(500, "El padrón que intenta actualizar no existe en la base de datos");

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
                await _padron.Delete(id);
                return Ok("Padrón eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Padrón no encontrado"))
                    return StatusCode(500, "El padrón que intenta eliminar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
