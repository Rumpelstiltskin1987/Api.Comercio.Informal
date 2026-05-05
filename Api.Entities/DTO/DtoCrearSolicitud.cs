using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities.DTO
{
    public class DtoCrearSolicitud
    {
        [Required]
        public int IdRecaudacion { get; set; } 

        [Required]
        public int IdUsuarioSolicita { get; set; } 

        [Required]
        public string MotivoSolicitud { get; set; } 
    }
}
