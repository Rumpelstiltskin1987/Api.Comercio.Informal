using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoGremio
    {
        public int IdGremio { get; set; }
        public required string Descripcion { get; set; }
    }
}
