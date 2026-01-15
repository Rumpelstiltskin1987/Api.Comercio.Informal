using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoRecaudacionDetalle
    {

        public int Id { get; set; }
        public required string FolioRecibo { get; set; }
        public required string NombreContribuyente { get; set; }
        public required string MatriculaContribuyente { get; set; }
        public required string GremioContribuyente { get; set; }
        public required string Concepto {  get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCobro { get; set; }
        public required string NombreCobrador { get; set; }
        public required string Estado {  get; set; }

    }
}
