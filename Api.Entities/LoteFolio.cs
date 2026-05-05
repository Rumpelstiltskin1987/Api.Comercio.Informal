using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class LoteFolio
    {
        public int Id_lote { get; set; }
        public int Id_usuario { get; set; }
        public int Id_gremio { get; set; }
        public int Rango_inicial { get; set; }
        public int Rango_final { get; set; }
        public int Ultimo_usado { get; set; }
        public int Anio { get; set; }
        public string Estado { get; set; } = "ACTIVO";
        public DateTime Fecha_asignacion { get; set; } = DateTime.UtcNow;
        public DateTime? Fecha_modificacion { get; set; }

    }
}
