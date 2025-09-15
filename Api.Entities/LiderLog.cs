using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class LiderLog
    {
        public int id_movimiento { get; set; }
        public int id_lider { get; set; }
        public string nombre { get; set; }
        public string a_paterno { get; set; }
        public string a_materno { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public string? direccion { get; set; }
        public string estado { get; set; }
        public string? Tipo_movimiento { get; set; }
        public string? usuario_modificacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
    }
}
