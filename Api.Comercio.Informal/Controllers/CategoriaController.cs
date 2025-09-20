using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;

namespace Api.Comercio.Informal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController(ILogger<CategoriaController> logger, MySQLiteContext context) : ControllerBase
    {
        private readonly BusinessCategoria _categoria = new(context);
        private readonly ILogger<CategoriaController> _logger = logger;

        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Categoria> categorias;
            try
            {
                categorias = await _categoria.GetAll();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(categorias);
        }

        [Route("GetById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id incorrecto.");
            }

            Entities.Categoria categoria;

            try
            {
                categoria = await _categoria.GetById(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(categoria);
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string usuario)
        {
            if (nombre == null || usuario == null)
            {
                return BadRequest("Datos incompletos. Verifique.");
            }

            try
            {
                Categoria categoria = new()
                {
                    Nombre = nombre,
                    Usuario_alta = usuario
                };
                var result = await _categoria.Create(categoria);
                return Ok("Categoria creada correctamente");
            }
            catch (Exception ex)
            {
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
                var result = await _categoria.Delete(id);
                return Ok("Categoria eliminada correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Categoria no encontrada"))
                    return StatusCode(500, "La categoría que intenta eliminar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, string nombre, string status, string usuario)
        {
            if (nombre == null || status == null || usuario == null)
            {
                return BadRequest("Datos incompletos. Verifique.");
            }

            try
            {
                var result = await _categoria.Update(id, nombre, status, usuario);
                return Ok("Categoria actualizada correctamente");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Categoria no encontrada"))
                    return StatusCode(500, "La categoría que intenta actualizar no existe en la base de datos");

                return StatusCode(500, ex.Message);
            }
        }
    }
}
