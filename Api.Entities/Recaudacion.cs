using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Recaudacion
    {
        public int Id_recaudacion { get; set; }
        [ForeignKey("Padron")]
        public int Id_padron { get; set; }
        public virtual Padron Padron { get; set; }
        [ForeignKey("Concepto")]
        public int Id_concepto { get; set; }
        public virtual Concepto Concepto { get; set; }
        public decimal Monto { get; set; }
        [ForeignKey("Cobrador")]
        public int Id_cobrador { get; set; }
        public virtual Usuario? Cobrador { get; set; }  
        public DateTime Fecha_cobro { get; set; }
        public string Folio_Recibo { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, C: Cancelado
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public DateTime Fecha_Alta {  get; set; } = DateTime.UtcNow;   


    }
}