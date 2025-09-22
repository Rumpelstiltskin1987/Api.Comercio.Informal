using Api.Business;
using Api.Entities;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Comercio.Informal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConceptoController(ILogger<ConceptoController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessConcepto _concepto = new(context);
        private readonly ILogger<ConceptoController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Concepto> Conceptos;
            try
            {
                Conceptos = await _concepto.GetAll();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(Conceptos);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Entities.Concepto Concepto;

            try
            {
                Concepto = await _concepto.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(Concepto);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string descripcion, string usuario)
        {
            if (descripcion == null || usuario == null)
            {
                return BadRequest("Datos incompletos. Verifique.");
            }

            try
            {
                Concepto Concepto = new()
                {
                    Descripcion = descripcion,
                    Usuario_alta = usuario
                };
                var result = await _concepto.Create(Concepto);
                return Ok("Concepto creado correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string descripcion, string status, string usuario)
        {
            if (descripcion == null || status == null || usuario == null)
            {
                return BadRequest("Datos incompletos. Verifique.");
            }

            try
            {
                Concepto newConcepto = new()
                {
                    Descripcion = descripcion,
                    Estado = status,
                    Usuario_alta = usuario,
                    Usuario_modificacion = usuario
                };
                var result = await _concepto.Update(id, newConcepto);
                return Ok("Concepto actualizado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Concepto no encontrado"))
                    return StatusCode(500, "La concepto que intenta actualizar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string usuario)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            try
            {
                var result = await _concepto.Delete(id, usuario);
                return Ok("Concepto eliminado correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Concepto no encontrado"))
                    return StatusCode(500, "El concepto que intenta eliminar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }

    }
}
