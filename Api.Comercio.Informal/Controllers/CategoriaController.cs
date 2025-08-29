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
        private readonly BusinessCategoria _categoria;
        public CategoriaController(ILogger<CategoriaController> logger, BusinessCategoria categoria,
            MySQLiteContext context)
        {
            _logger = logger;
            _context = context;
            _categoria = new (_context);
            
        }

        [HttpGet(Name = "GetCategorias")]
        public async Task<IActionResult> Get()
        {
            var categorias = await _categoria.GetAll();
            foreach (Categoria categoria in categorias)
            {
                categoria.Fecha_alta = DateTime.Now;
            }
            return Ok(categorias);
        }

        [HttpPost(Name = "CreateCategoria")]
        public async Task<IActionResult> Create([FromBody] Categoria categoria)
        {
            if (categoria == null)
            {
                return BadRequest("La categoria no puede ser nula");
            }
            var result = await _categoria.Create(categoria);
            if (result)
            {
                return Ok("Categoria creada correctamente");
            }
            else
            {
                return StatusCode(500, "Error al crear la categoria");
            }
        }
    }
}
