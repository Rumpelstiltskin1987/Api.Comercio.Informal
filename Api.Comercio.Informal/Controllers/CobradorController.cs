using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Api.Entities;
using Api.Business;

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

        [Route("GetCobradores")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Entities.Cobrador> cobradores;
            try
            {
                cobradores = await _cobrador.GetAll();

                foreach (Entities.Cobrador cobrador in cobradores)
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
    }
}
