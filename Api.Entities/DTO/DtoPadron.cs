using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoPadron
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string APaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        public string AMaterno { get; set; }

        [Required(ErrorMessage = "La CURP es obligatoria")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "La CURP debe tener 18 caracteres")]
        public string Curp { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; }
  
        public string? Email { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar un gremio válido")]
        public int IdGremio { get; set; }

        [Required(ErrorMessage = "El tipo de comerciante es obligatorio")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario que registra es obligatorio")]
        public string Usuario { get; set; } = string.Empty;
    }
}
