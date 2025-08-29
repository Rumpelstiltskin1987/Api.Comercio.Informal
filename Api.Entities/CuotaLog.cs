using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class CuotaLog
    {
        public int Id_movimiento { get; set; }
        public int Id_cuota { get; set; }
        public decimal Monto { get; set; }
        public required string Estado { get; set; }
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
