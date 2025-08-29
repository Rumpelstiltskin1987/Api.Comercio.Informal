using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class GremioLog
    {
        public int id_movimiento { get; set; }
        public int id_gremio { get; set; }
        public string descripcion { get; set; }
        public int id_lider { get; set; }
        public string estado { get; set; }
        public string? usuario_modificacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
    }
}
