using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Gremio
    {
        public int Id_gremio { get; set; }
        public string Descripcion { get; set; }
        public int Id_lider { get; set; }
        public virtual Lider? Lider { get; set; }
        public string Estado { get; set; } = "A"; // A: Activo, I: Inactivo
        public string Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; }
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
