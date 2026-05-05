using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoTarifa
    {
        public int IdTarifa { get; set; }
        public int IdConcepto { get; set; }
        public int IdGremio { get; set; }
        public decimal Monto { get; set; }
    }
}
