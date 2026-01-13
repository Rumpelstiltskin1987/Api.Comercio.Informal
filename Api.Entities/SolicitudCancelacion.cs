using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class SolicitudCancelacion
    {
        [Key]
        public int Id_solicitud { get; set; }
        [ForeignKey("Recaudacion")]
        public int Id_recaudacion { get; set; }        
        public Recaudacion? Recaudacion { get; set; }

        // Quién pide cancelar (El cobrador)
        [ForeignKey("UsuarioSolicita")]
        public int Id_usuario_solicita { get; set; }
        public virtual Usuario? UsuarioSolicita { get; set; }
        public DateTime Fecha_solicitud { get; set; } = DateTime.Now;
        public string Motivo_solicitud { get; set; }

        // Estado del trámite: "P = PENDIENTE", "A= APROBADO", "R = RECHAZADO"
        public string Estado_solicitud { get; set; } = "P";

        public int Id_usuario_responde { get; set; }        
        public DateTime? Fecha_respuesta { get; set; }
        public string? Motivo_respuesta { get; set; } 
    }
}
