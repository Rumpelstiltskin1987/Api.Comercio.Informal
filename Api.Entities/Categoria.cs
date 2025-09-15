using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Api.Entities
{
    public class Categoria
    {
        public int Id_categoria { get; set; }
        public string? Nombre { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, I: Inactivo
        public  string? Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; } = DateTime.Now;
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
