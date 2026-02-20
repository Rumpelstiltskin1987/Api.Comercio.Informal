using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business.Services
{
    public interface IEmailService
    {
        Task EnviarCredencialesAsync(string emailDestino, string nombreUsuario, string passwordTemporal, bool esReseteo = false);
    }
}
