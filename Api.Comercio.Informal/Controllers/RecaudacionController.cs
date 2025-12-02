using Api.Business;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecaudacionController(ILogger<RecaudacionController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessRecaudacion _recaudacion = new(context);
        private readonly ILogger<RecaudacionController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Recaudacion> recaudacion;

            try
            {
                recaudacion = await _recaudacion.GetAll();

                if (!recaudacion.Any())
                    throw new Exception("No existen registros en la base de datos.");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(recaudacion);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }
            Recaudacion cobro;
            try
            {
                cobro = await _recaudacion.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(cobro);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(int id_padron, int id_concepto, decimal monto,
            int id_cobrador, string folio_Recibo, string periodo_Inicio, string periodo_Fin, 
            int id_tarifa, double? latitud, double? longitud)
        {            
            try
            {
                await _recaudacion.Create(id_padron, id_concepto, monto,
            id_cobrador, folio_Recibo, periodo_Inicio, periodo_Fin,
            id_tarifa, latitud, longitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok("Recaudación registrada correctamente");
        }
    }
}
