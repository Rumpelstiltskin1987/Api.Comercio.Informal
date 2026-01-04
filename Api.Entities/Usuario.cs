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
        public string Nombre { get; set; }
        public string A_paterno { get; set; }
        public string A_materno { get; set; }
        public string? Alias { get; set; }
        public string? Usuario_alta { get; set; }
        public DateTime? Fecha_alta { get; set; } = DateTime.Now;
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
        public bool EsPasswordTemporal { get; set; } = false;
    }
}
