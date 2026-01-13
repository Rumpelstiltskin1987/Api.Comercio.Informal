using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoBandejaSolicitud
    {
        public int Id_solicidud { get; set; }
        public string Folio_recibo { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }

        // Datos del solicitante
        public string Nombre_cobrador { get; set; }

        // Datos de la solicitud
        public DateTime Fecha_solicitud { get; set; }
        public string Motivo_solicitud { get; set; }

        // Estado (P, A, R)
        public string Estado_solicitud { get; set; }

        // Auxiliar para mostrar el estado bonito en el UI
        public string Estado_descripcion => Estado_solicitud switch
        {
            "P" => "Pendiente",
            "A" => "Aprobado",
            "R" => "Rechazado",
            _ => "Desconocido"
        };
    }
}
