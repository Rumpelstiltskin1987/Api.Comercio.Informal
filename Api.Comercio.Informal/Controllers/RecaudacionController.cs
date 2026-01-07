using Api.Business;
using Api.Entities;
using Api.Entities.DTO;
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

        [Route("GetByFolio")]
        [HttpPost]
        public async Task<IActionResult> GetByFolio([FromBody] DtoBusquedaFolio request) 
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Folio))
            {
                return BadRequest(new { message = "El folio es obligatorio." });
            }

            try
            {
                var cobro = await _recaudacion.GetByFolio(request.Folio);

                if (cobro == null)
                {
                    return NotFound(new { message = "No se encontró el folio especificado." });
                }

                return Ok(cobro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Search")]
        [HttpGet]
        public async Task<IActionResult> Search(int? idCobrador, int? idConcepto, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                var afiliados = await _recaudacion.Search(idCobrador, idConcepto, fechaInicio, fechaFin);

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
        public async Task<IActionResult> Create([FromBody] DtoRecaudacion request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _recaudacion.Create(
                    request.IdPadron,
                    request.IdGremio,
                    request.IdConcepto,
                    request.Monto,
                    request.IdCobrador,
                    request.Latitud,
                    request.Longitud
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(new { mensaje = "Recaudación registrada correctamente" }); ;
        }
    }
}
