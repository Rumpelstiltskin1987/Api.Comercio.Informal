using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;
using System.Runtime.CompilerServices;

namespace Api.Comercio.Informal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiderController(ILogger<LiderController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessLider _lider = new(context);
        private readonly ILogger<LiderController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Lider> lideres;

            try
            {
                lideres = await _lider.GetAll();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(lideres);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Lider lider;

            try
            {
                lider = await _lider.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(lider);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string a_paterno, string a_materno, string telefono,
            string email, string direccion, string usuario)
        {
            try
            {
                await _lider.Create(nombre, a_paterno, a_materno, telefono,
                    email, direccion, usuario);
                return Ok("Lider creado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string nombre, string a_paterno, string a_materno, string telefono,
            string email, string direccion, string status, string usuario)
        {
            try
            {
                await _lider.Update(id, nombre, a_paterno, a_materno, telefono,
                    email, direccion, status, usuario);
                return Ok("Lider actualizado correctamente.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lider no encontrado"))
                    return StatusCode(500, "El lider que intenta actualizar no existe en la base de datos");

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
                await _lider.Delete(id);
                return Ok("Lider eliminado correctamente.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lider no encontrado"))
                    return StatusCode(500, "El lider que intenta actualizar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
