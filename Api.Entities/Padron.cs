using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Padron
    {
        public int Id_padron { get; set; }        
        public string Nombre { get; set; }
        public string A_paterno { get; set; }
        public string A_materno { get; set; }
        public string Curp { get; set; }
        public string Direccion { get; set; } 
        public string Telefono { get; set; }
        public string? Email { get; set; }
        public string Matricula { get; set; }
        public string? Matricula_anterior { get; set; }
        public int Id_gremio { get; set; } = 0;
        public string Tipo_vendedor { get; set; } = "0"; // P=Padron, E=Eventual
        public string Estado { get; set; }  = "A"; // A: Activo, I: Inactivo  
        public string? Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; }
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
