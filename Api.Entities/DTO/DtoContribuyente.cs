using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoContribuyente
    {
        public int IdContribuyente { get; set; }
        public required string Matricula { get; set; }
        public required string Nombre { get; set; }
        public required string APaterno { get; set; }
        public required string AMaterno { get; set; }
        public required string Curp { get; set; }
        public required string Tipo { get; set; }
        public int IdGremio { get; set; }
        public required string Gremio { get; set; }
    }
}
