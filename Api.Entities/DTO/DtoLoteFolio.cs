using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoLoteFolio
    {
        public int IdLote { get; set; }
        public int IdUsuario { get; set; }
        public int IdGremio { get; set; }
        public required string Prefijo { get; set; }
        public int Anio { get; set; }
        public int FolioInicial  { get; set; }
        public int FolioFinal    { get; set; }   
        public int UltimoUsado { get; set; }
    }
}
