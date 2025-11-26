using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;

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
            IEnumerable<Padron> padrones;

            try
            {
                padrones = await _padron.GetAll();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(padrones);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Padron padron;

            try
            {
                padron = await _padron.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(padron);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string a_paterno, string a_materno, string curp,
            string direccion, string telefono, string? email, int id_gremio, string tipo, string usuario)
        {
            try
            {                
                await _padron.Create(nombre, a_paterno, a_materno, curp,
            direccion, telefono, email, id_gremio, tipo, usuario);
                return Ok("Padron creado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                return Ok("Padrón actualizado correctamente");
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
