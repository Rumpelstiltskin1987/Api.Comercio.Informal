using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class PadronLog
    {
        public int Id_movimiento { get; set; }
        public int Id_padron { get; set; }
        public string? Matricula { get; set; }
        public string? Nombre { get; set; }
        public string? A_paterno { get; set; }
        public string? A_materno { get; set; }
        public string? Curp { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public int Id_gremio { get; set; }
        public string? Estado { get; set; }
        public string? Tipo_movimiento { get; set; }
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
