using Api.Business;
using Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolioController(ILogger<FolioController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessFolio _folio = new(context);
        private readonly ILogger<FolioController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Folio> folios;
            
            try
            {
                folios = await _folio.GetAll();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(folios);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Folio folio;

            try
            {
                folio = await _folio.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(folio);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(int id_gremio, string descripcion, string prefijo)
        {
            try
            {
                await _folio.Create(id_gremio, descripcion, prefijo);
                return Ok("Folio creado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }            
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, int id_gremio, string descripcion, string prefijo,
            int siguiente_folio, int anio_vigente)
        {
            try
            {
                await _folio.Update(id, id_gremio, descripcion, prefijo, siguiente_folio, anio_vigente);
                return Ok("Folio actualizado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Folio no encontrado"))
                    return StatusCode(500, "El folio que intenta actualizar no existe en la base de datos");

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
                await _folio.Delete(id);
                return Ok("Folio eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Folio no encontrada"))
                    return StatusCode(500, "El folio que intenta eliminar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
