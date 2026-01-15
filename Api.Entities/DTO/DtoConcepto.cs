using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Entities.DTO
{
    public class DtoConcepto
    {
        public int IdConcepto { get; set; }
        public required string Descripcion { get; set; }
    }
}
