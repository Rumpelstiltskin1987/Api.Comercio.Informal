using Api.Business.Services;
using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business
{
    public class BusinessUsuario : IUsuario
    {
        private readonly MySQLiteContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly DataUsuario _usuario;
        private readonly DataUsuarioLog _usuarioLog;
        private readonly IEmailService _emailService;

        public BusinessUsuario(UserManager<Usuario> userManager, MySQLiteContext context, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _usuario = new DataUsuario(_userManager, _context);
            _usuarioLog = new DataUsuarioLog(_context);
            _emailService = emailService;
        }
        public async Task<IEnumerable<DtoUsuario>> GetAll()
        {
            var users = await _usuario.GetAllExcludingSuperadmin();
            

            return users;
        }

        public async Task<Usuario> GetById(string id)
        {
            return await _usuario.GetById(id);
        }

        public async Task<IEnumerable<Usuario>> GetByRol(string rol)
        {
            return await _usuario.GetByRol(rol);
        }
        
        public async Task Create(DtoUsuario inputmodel)
        {
            // 1. Generamos el password en el backend
            string passwordTemporal = GenerarPasswordTemporal();

            var nuevoUsuario = new Usuario
            {
                UserName = inputmodel.UserName,

                Nombre = inputmodel.Nombre,
                A_paterno = inputmodel.A_paterno,
                A_materno = inputmodel.A_materno,
                Email = inputmodel.Email,
                PhoneNumber = inputmodel.PhoneNumber,
                Estado = inputmodel.Estado,
                Usuario_alta = inputmodel.Usuario_alta,
                EsPasswordTemporal = true
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _usuario.Create(nuevoUsuario, passwordTemporal, inputmodel.Rol);

                // Registrar el log de creación

                UsuarioLog log = new()
                {
                    Id_movimiento = 1,
                    Id = nuevoUsuario.Id,
                    UserName = nuevoUsuario.UserName,
                    Nombre = nuevoUsuario.Nombre,
                    A_paterno = nuevoUsuario.A_paterno,
                    A_materno = nuevoUsuario.A_materno,
                    Email = nuevoUsuario.Email,
                    PhoneNumber = nuevoUsuario.PhoneNumber,
                    Estado = nuevoUsuario.Estado,
                    Rol = inputmodel.Rol,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = nuevoUsuario.Usuario_alta,
                    Fecha_modificacion = nuevoUsuario.Fecha_alta
                };

                await _usuarioLog.AddLog(log);                

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            // 3. BLOQUE DE NOTIFICACIÓN EXTERNA (Post-Commit)
            try
            {
                await _emailService.EnviarCredencialesAsync(nuevoUsuario.Email, nuevoUsuario.UserName, passwordTemporal, false);
            }
            catch (Exception ex)
            {
                throw new Exception("El usuario se creó, pero no se pudieron enviar las credenciales por correo. Por favor, contacte al administrador.");
            }
        }

        public async Task Update(DtoUsuario inputmodel)
        {
            Usuario usuarioExistente = await _usuario.GetById(inputmodel.Id.ToString());

            usuarioExistente.UserName = inputmodel.UserName;
            usuarioExistente.Nombre = inputmodel.Nombre;
            usuarioExistente.A_paterno = inputmodel.A_paterno;
            usuarioExistente.A_materno = inputmodel.A_materno;
            usuarioExistente.Email = inputmodel.Email;
            usuarioExistente.PhoneNumber = inputmodel.PhoneNumber;
            usuarioExistente.Estado = inputmodel.Estado;
            usuarioExistente.Usuario_modificacion = inputmodel.Usuario_modificacion;
            usuarioExistente.Fecha_modificacion = DateTime.UtcNow;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _usuario.Update(usuarioExistente);
                await _usuario.CambiarRol(usuarioExistente, inputmodel.Rol);
                var idmovimiento = await _usuarioLog.GetIdMovement(usuarioExistente.Id) + 1;

                UsuarioLog log = new()
                {
                    Id_movimiento = idmovimiento,
                    Id = usuarioExistente.Id,
                    UserName = usuarioExistente.UserName,
                    Nombre = usuarioExistente.Nombre,
                    A_paterno = usuarioExistente.A_paterno,
                    A_materno = usuarioExistente.A_materno,
                    Email = usuarioExistente.Email,
                    PhoneNumber = usuarioExistente.PhoneNumber,
                    Estado = usuarioExistente.Estado,
                    Rol = inputmodel.Rol,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = usuarioExistente.Usuario_modificacion,
                    Fecha_modificacion = usuarioExistente.Fecha_modificacion
                };

                await _usuarioLog.AddLog(log);                
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<DtoHistorial>> GetHistorial(int id)
        {
            try
            {
                // 1. Obtenemos la lista cruda de la base de datos
                var logs = await _usuarioLog.GetLogsByUserId(id);

                // 2. Transformamos (Mapeamos) cada UsuarioLog a DtoHistorial
                var historial = logs.Select(log => new DtoHistorial
                {
                    Fecha = log.Fecha_modificacion,
                    Usuario = log.Usuario_modificacion,
                    Movimiento = log.Tipo_movimiento.ToUpper() switch
                    {
                        "A" => "Alta",
                        "M" => "Modificación",
                        _ => log.Tipo_movimiento 
                    },
                    Detalles = $"Nombre: {log.Nombre} {log.A_paterno} {log.A_materno} " +
                               $"| Rol: {log.Rol} " +
                               $"| Estado: {(log.Estado == "A" ? "Activo" : (log.Estado == "I" ? "Inactivo" : log.Estado))} " +
                               $"| Email: {log.Email}"
                }).ToList();

                return historial;
            }   
            catch (Exception ex)
            {
                // Es buena práctica loguear el error antes de lanzarlo, si tienes un logger
                throw new Exception("Error al obtener el historial", ex);
            }
        }

        private string GenerarPasswordTemporal()
        {
            // Genera un password seguro de 10 caracteres (Ej: K7#mP9z!xQ)
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Aseguramos que cumpla las reglas básicas de Identity
            char[] chars = new char[10];
            chars[0] = "ABCDEFGHJKLMNOPQRSTUVWXYZ"[random.Next(25)];
            chars[1] = "abcdefghijklmnopqrstuvwxyz"[random.Next(26)];
            chars[2] = "0123456789"[random.Next(10)];
            chars[3] = "!@#$%^&*?_-"[random.Next(11)];

            for (int i = 4; i < 10; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }

            return new string(chars.OrderBy(x => random.Next()).ToArray());
        }

        public async Task ResetearPassword(string idUsuario, string usuarioQueEjecuta)
        {
            var user = await _userManager.FindByIdAsync(idUsuario);
            if (user == null) throw new Exception("Usuario no encontrado.");

            // 1. Generamos nueva credencial
            string nuevoPasswordTemporal = GenerarPasswordTemporal();

            // 2. Generamos el token de seguridad de Identity
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // 3. Aplicamos el reseteo
            var result = await _userManager.ResetPasswordAsync(user, resetToken, nuevoPasswordTemporal);

            if (result.Succeeded)
            {
                // 4. Volvemos a encender la bandera de obligatoriedad
                user.EsPasswordTemporal = true;
                await _userManager.UpdateAsync(user);

                // 5. Registrar en el Historial (Log)
                // Guardar en DataUsuarioLog que 'usuarioQueEjecuta' reseteó el password de 'user.Nombre'

                // 6. Enviar Correo
                //await _emailService.EnviarCredencialesAsync(user.Email, user.UserName, nuevoPasswordTemporal);
                await _emailService.EnviarCredencialesAsync(user.Email, user.UserName, nuevoPasswordTemporal, true);
            }
            else
            {
                throw new Exception("Error al resetear la contraseña: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
