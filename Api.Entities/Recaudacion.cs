using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Recaudacion
    {
        public int Id_recaudacion { get; set; }
        public int Id_padron { get; set; }
        public int Id_concepto { get; set; }
        public decimal Monto { get; set; }
        public int Id_cobrador { get; set; }
        public DateTime Fecha_cobro { get; set; } = DateTime.Now;
        public string Folio_Recibo { get; set; }
        public int? Id_tarifa { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, C: Cancelado
        public string Periodo_Inicio { get; set; }
        public string Periodo_Fin { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }
}