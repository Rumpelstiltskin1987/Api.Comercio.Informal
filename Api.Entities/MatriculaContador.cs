using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class MatriculaContador
    {        
        public string Tipo_vendedor { get; set; }
        public int Anio { get; set; }
        public int Siguiente_numero { get; set; } = 1;
    }
}
