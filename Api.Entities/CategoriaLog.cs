using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class CategoriaLog
    {
        public int Id_movimiento { get; set; }
        public int Id_categoria { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Usuario_modificacion { get; set; }
        public DateTime Fecha_modificacion { get; set; } = DateTime.Now;
    }
}
