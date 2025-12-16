using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifaController(ILogger<TarifaController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessTarifa _tarifa = new BusinessTarifa(context);
        private readonly ILogger<TarifaController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Tarifa> tarifas;
            try
            {
                tarifas = await _tarifa.GetAll();

                if (!tarifas.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(tarifas);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Tarifa tarifa;

            try
            {
                tarifa = await _tarifa.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(tarifa);
        }

        [Route("Search")]
        [HttpGet]
        public async Task<IActionResult> Search(int? id_concept, int? id_gremio, decimal? monto, string? estado)
        {
            try
            {
                var tarifas = await _tarifa.Search(id_concept, id_gremio, monto, estado);
                return Ok(tarifas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(int id_concepto, int id_gremio, decimal? monto, string usuario)
        {
            try
            {
                await _tarifa.Create(id_concepto, id_gremio, monto, usuario);
                return Ok("Tarifa registrada correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, int id_concepto, int id_gremio, decimal? monto, string estado, string usuario)
        {
            try
            {
                await _tarifa.Update(id, id_concepto, id_gremio, monto, estado,  usuario);
                return Ok("Tarifa actualizada correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Tarifa no encontrada"))
                    return StatusCode(500, "La tarifa que intenta actualizar no existe en la base de datos");

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
                await _tarifa.Delete(id);
                return Ok("Tarifa eliminada correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Tarifa no encontrada"))
                    return StatusCode(500, "La tarifa que intenta eliminar no existe en la base de datos");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
