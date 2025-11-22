using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Cobrador
    {
        public int Id_cobrador { get; set; }
        public required string Nombre { get; set; }
        public required string A_paterno { get; set; }
        public required string A_materno { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Estado { get; set; } = "A"; // A: Activo, I: Inactivo
        public string? Usuario_alta { get; set; }
        public DateTime? Fecha_alta { get; set; } = DateTime.Now;
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
} 
