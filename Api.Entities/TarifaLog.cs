using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class TarifaLog
    {
        public int Id_movimiento { get; set; }
        public int Id_tarifa { get; set; }
        public int Id_concepto { get; set; }
        public int? Id_gremio { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public string Tipo_movimiento { get; set; }
        public string Usuario_modificacion { get; set; }
        public DateTime Fecha_modificacion { get; set; }
    }
}