using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities
{
    public class UsuarioLog
    {
        public required int Id_movimiento { get; set; }
        public required string UserName { get; set; }
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public required string A_paterno { get; set; }
        public required string A_materno { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Rol { get; set; }
        public required string Estado { get; set; }
        public required string Tipo_movimiento { get; set; }
        public string? Usuario_modificacion { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
    }
}
