using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Business;
using Api.Entities;

namespace Api.Comercio.Informal.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ILogger<CategoriaController> _logger;
        private readonly MySQLiteContext _context;
        private readonly Business.BusinessCategoria _categoria;
        public CategoriaController(ILogger<CategoriaController> logger, Business.BusinessCategoria categoria,
            MySQLiteContext context)
        {
            _logger = logger;
            _context = context;
            _categoria = new(_context);

        }

        [Route("GetCategorias")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Entities.Categoria> categorias;
            try
            {
                categorias = await _categoria.GetAll();

                foreach (Entities.Categoria categoria in categorias)
                {
                    categoria.Fecha_alta = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(categorias);
        }

        [Route("GetCategoriaById")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id de la categoria es necesario.");
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
        public async Task<IActionResult> Create(string categoryName, string user)
        {
            if (categoryName == null)
            {
                return BadRequest("El nombre de la categoria es necesario.");
            }

            if (categoryName == null)
            {
                return BadRequest("El usuario es necesario.");
            }

            try
            {
                Entities.Categoria categoria = new()
                {
                    Descripcion = categoryName,
                    Usuario_alta = user
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
                return BadRequest("El id de la categoria es necesario.");
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
        public async Task<IActionResult> Update(int id, string categoryName, string status)
        {            
            if (categoryName == null)
            {
                return BadRequest("El nombre de la categoria es necesario.");
            }

            if (id <= 0)
            {
                return BadRequest("El id de la categoria es necesario.");
            }

            Entities.Categoria categoria;

            try
            {
                var result = await _categoria.Update(id, categoryName, status);
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
