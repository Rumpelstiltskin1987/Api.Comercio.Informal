using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoUsuario
    {
        public int Id { get; set; } // Para saber si es edición o nuevo

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string A_paterno { get; set; } = "";

        public string A_materno { get; set; } = "";

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = "";
        [Required(ErrorMessage = "El Rol es obligatorio")]
        public string? Rol { get; set; }
    }
}
