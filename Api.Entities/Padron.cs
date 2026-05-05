using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Padron
    {
        public int Id_padron { get; set; }        
        public required string Nombre { get; set; }
        public required string A_paterno { get; set; }
        public required string A_materno { get; set; }
        public required string Curp { get; set; }
        public required string Direccion { get; set; } 
        public required string Telefono { get; set; }
        public string? Email { get; set; }
        public required string Matricula { get; set; }
        public string? Matricula_anterior { get; set; }
        [ForeignKey("Gremio")]
        public int Id_gremio { get; set; }
        public virtual Gremio? Gremio { get; set; }
        public string Tipo_vendedor { get; set; } = "0"; // P=Padron, E=Eventual
        public string Estado { get; set; }  = "A"; // A: Activo, I: Inactivo  
        public string? Usuario_alta { get; set; }
        public DateTime Fecha_alta { get; set; } = DateTime.UtcNow;
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
