using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarCredencialesAsync(string emailDestino, string nombreUsuario, string passwordTemporal, bool esReseteo = false)
        {
            var smtpSettings = _config.GetSection("SmtpSettings");

            string host = smtpSettings["Host"]!;
            int port = int.Parse(smtpSettings["Port"]!);
            bool enableSsl = bool.Parse(smtpSettings["EnableSsl"]!);
            string emailOrigen = smtpSettings["Email"]!;
            string password = smtpSettings["Password"]!;
            string displayName = smtpSettings["DisplayName"]!;

            string asunto = esReseteo
                ? "SIRCIN - Reseteo de Contraseña"
                : "SIRCIN - Bienvenido, tu cuenta ha sido creada";

            // Plantilla HTML profesional básica
            string cuerpoHtml = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #ddd; padding: 20px; border-radius: 8px;'>
                    <h2 style='color: #198754; text-align: center;'>SIRCIN</h2>
                    <p>Hola,</p>
                    <p>{(esReseteo ? "Se ha solicitado un reseteo de tu contraseña." : "Tu cuenta ha sido creada exitosamente en el sistema.")}</p>
                    <p>Tus credenciales de acceso son:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p style='margin: 0;'><strong>Usuario:</strong> {nombreUsuario}</p>
                        <p style='margin: 0;'><strong>Contraseña Temporal:</strong> {passwordTemporal}</p>
                    </div>
                    <p style='color: #dc3545; font-size: 0.9em;'>
                        <em>⚠️ Por motivos de seguridad, el sistema te solicitará cambiar esta contraseña en tu primer inicio de sesión.</em>
                    </p>
                    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;' />
                    <p style='font-size: 0.8em; color: #888; text-align: center;'>
                        Este es un correo automático, por favor no respondas a este mensaje.
                    </p>
                </div>";

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(emailOrigen, password),
                EnableSsl = enableSsl
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(emailOrigen, displayName),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };

            mailMessage.To.Add(emailDestino);

            // Enviamos el correo de forma asíncrona
            await client.SendMailAsync(mailMessage);
        }
    }
}
