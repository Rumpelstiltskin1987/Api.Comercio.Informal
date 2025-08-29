using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Padron
    {
        public int id_padron { get; set; }
        public string matricula { get; set; }
        public string nombre { get; set; }
        public string a_paterno { get; set; }
        public string a_materno { get; set; }
        public string? curp { get; set; }
        public string? direccion { get; set; } 
        public string? telefono { get; set; }
        public string? email { get; set; }
        public int id_gremio { get; set; }
        public string usuario_alta { get; set; }
        public DateTime fecha_alta { get; set; }
        public string? usuario_modificacion { get; set; }
        public DateTime? fecha_modificacion { get; set; }
    }
}
