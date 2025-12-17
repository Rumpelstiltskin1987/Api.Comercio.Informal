using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoRecaudacion
    {
        [Required(ErrorMessage = "El IdPadron es obligatorio")]
        public int IdPadron { get; set; }

        [Required(ErrorMessage = "El IdGremio es obligatorio")]
        public int IdGremio { get; set; }

        [Required(ErrorMessage = "El IdConcepto es obligatorio")]
        public int IdConcepto { get; set; }

        [Required(ErrorMessage = "El IdGremio es obligatorio")]
        [Range(0.01, 999999, ErrorMessage = "El Monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El IdCobrador es obligatorio")]
        public int IdCobrador { get; set; }

        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

    }
}
