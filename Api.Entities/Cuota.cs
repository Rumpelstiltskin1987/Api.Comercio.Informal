using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Cuota
    {
        public int Id_cuota { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, I: Inactivo
        public required string Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; }
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
