using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class Usuario : IdentityUser<int>
    {
        // Campo para vincular el login con tu tabla de Cobradores
        public int? Id_cobrador { get; set; }

        // Propiedad de navegación (Opcional, para traer datos del cobrador fácilmente)
        [ForeignKey("Id_cobrador")]
        public virtual Cobrador? Cobrador { get; set; }

        // Campo adicional para la App de Android (ejemplo: un alias o nombre corto)
        public string? Alias { get; set; }

        // Podrías agregar otros campos como:
        // public string? Turno { get; set; }
        // public bool EsAdministrador { get; set; }
    }
}
