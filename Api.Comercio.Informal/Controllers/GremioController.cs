using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GremioController(ILogger<GremioController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessGremio _gremio = new(context);
        private readonly ILogger<GremioController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Gremio> gremios;

            try
            {
                gremios = await _gremio.GetAll();

                if (!gremios.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(gremios);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Gremio gremio;

            try
            {
                gremio = await _gremio.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(gremio);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string descripcion, int id_lider, string usuario)
        {
            try
            {

                await _gremio.Create(descripcion, id_lider, usuario);
                return Ok("Gremio creado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string descripcion, int id_lider, string status, string usuario)
        {
            Gremio gremio = await _gremio.GetById(id);

            try
            {
                await _gremio.Update(id, descripcion, id_lider, status, usuario);
                return Ok("Gremio actualizado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Gremio no encontrado"))
                    return StatusCode(500, "El gremio que intenta actualizar no existe en la base de datos");

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
                await _gremio.Delete(id);
                return Ok("Gremio eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Gremio no encontrado"))
                    return StatusCode(404, "El gremio que intenta eliminar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
