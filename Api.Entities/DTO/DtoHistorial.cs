using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoHistorial
    {
        public DateTime? Fecha { get; set; }
        public required string Usuario { get; set; }
        public required string Movimiento { get; set; }
        public required string Detalles { get; set; }
    }
}
