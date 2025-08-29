using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Recaudacion
    {
        public int id_cobro { get; set; }
        public int id_padron { get; set; }
        public int id_concepto { get; set; }
        public decimal monto { get; set; }
        public int id_cobrador { get; set; }
        public DateTime fecha_cobro { get; set; }
        
    }
}
