using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Folio
    {
        public int Id_folio_serie { get; set; }
        public int? Id_gremio { get; set; }
        public string? Descripcion { get; set; }
        public string? Prefijo { get; set; }
        public int Siguiente_folio { get; set; } = 1;
        public int Anio_vigente { get; set; } = DateTime.Now.Year;
    }
}