using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoProcesarSolicitud
    {
        [Required]
        public int Id_solicidud { get; set; }

        [Required]
        public int Id_usuario_responde { get; set; } 

        [Required]
        public string Accion { get; set; } // "APROBAR" o "RECHAZAR"

        public string? Motivo_respuesta { get; set; } // Opcional, ej: "No procede"
    }
}
