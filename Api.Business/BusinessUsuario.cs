using Api.Interfaces;
using Api.Entities;
using Api.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Api.Entities.DTO;

namespace Api.Business
{
    public class BusinessUsuario : IUsuario
    {
        private readonly MySQLiteContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly DataUsuario _usuario;
        private readonly DataUsuarioLog _usuarioLog;

        public BusinessUsuario(UserManager<Usuario> userManager, MySQLiteContext context)
        {
            _context = context;
            _userManager = userManager;
            _usuario = new DataUsuario(_userManager, _context);
            _usuarioLog = new DataUsuarioLog(_context);
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
                await _usuario.Create(nuevoUsuario, inputmodel.Password, inputmodel.Rol);

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
            usuarioExistente.Fecha_modificacion = DateTime.Now;

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
    }
}
