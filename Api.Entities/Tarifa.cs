using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Tarifa
    {
        public int Id_tarifa { get; set; }
        [ForeignKey("Concepto")]
        public int Id_concepto { get; set; }
        public virtual Concepto? Concepto { get; set; }
        [ForeignKey("Gremio")]
        public int Id_gremio { get; set; }
        public virtual Gremio? Gremio { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, I: Inactivo
        public string Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; } = DateTime.Now;
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}