using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Folio
    {
        public int Id_folio_serie { get; set; }
        [ForeignKey("Gremio")]
        public int? Id_gremio { get; set; }
        public virtual Gremio? Gremio { get; set; }
        public string? Descripcion { get; set; }
        public string? Prefijo { get; set; }
        public int Siguiente_folio { get; set; } = 1;
        public int Anio_vigente { get; set; } = DateTime.Now.Year;
    }
}