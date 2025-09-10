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
        public DateTime Fecha_cobro { get; set; }
        
    }
}
