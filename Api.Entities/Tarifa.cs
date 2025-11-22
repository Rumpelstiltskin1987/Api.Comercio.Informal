using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Tarifa
    {
        public int Id_tarifa { get; set; }
        public int Id_concepto { get; set; }
        public int Id_gremio { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, I: Inactivo
        public string Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; } = DateTime.Now;
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}