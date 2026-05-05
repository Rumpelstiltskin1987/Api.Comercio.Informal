using Api.Entities;
using Api.Entities.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Data.Access
{
    public class DataUsuario
    {       
        private readonly UserManager<Usuario> _userManager;
        private readonly MySQLiteContext _context;

        public DataUsuario(UserManager<Usuario> userManager, MySQLiteContext context)
        {
            _userManager = userManager;
            _context = context;
        }   
        public async Task<IEnumerable<Usuario>> GetAll()
        {
            IEnumerable<Usuario> usuarios;

            try
            {
                usuarios = await _userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los usuarios: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los usuarios: " + ex.Message);
            }

            return usuarios;
        }

        public async Task<Usuario> GetById(string id)
        {
            Usuario? usuario;
            try
            {
                usuario = await _userManager.FindByIdAsync(id) ?? throw new Exception("Usuario no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el usuario: " + ex.InnerException.Message);
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
            return usuario;
        }
        
        public async Task<IEnumerable<Usuario>> GetByRol(string rol)
        {
            IEnumerable<Usuario>? usuarios;
            try
            {
                usuarios = await _userManager.GetUsersInRoleAsync(rol) ?? throw new Exception("No se encontraron usuarios con el rol especificado");

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los registros: " + ex.InnerException.Message);
                throw new Exception("Error al buscar los registros: " + ex.Message);
            }

            return usuarios;
        }

        public async Task<IEnumerable<DtoUsuario>> GetAllExcludingSuperadmin()
        {
            try
            {
                // LINQ Query syntax para mayor claridad en los JOINs
                var query = from u in _context.Users

                                // 1. Unimos con la tabla intermedia (UserRoles)
                            join ur in _context.UserRoles on u.Id equals ur.UserId into userRoles
                            from ur in userRoles.DefaultIfEmpty() // Left Join (para traer usuarios sin rol también)

                                // 2. Unimos con la tabla de Roles (AspNetRoles)
                            join r in _context.Roles on ur.RoleId equals r.Id into roles
                            from r in roles.DefaultIfEmpty()

                                // 3. APLICAMOS EL FILTRO (Excluir Superadmin)
                            where r.Name != "Superadmin"

                            // 4. PROYECCIÓN DIRECTA (Esto soluciona el problema de rendimiento)
                            // En lugar de traer la entidad pesada, llenamos el DTO aquí mismo.
                            select new DtoUsuario
                            {
                                Id = u.Id,
                                Nombre = u.Nombre,
                                A_paterno = u.A_paterno,
                                A_materno = u.A_materno,
                                PhoneNumber = u.PhoneNumber,
                                Email = u.Email,
                                UserName = u.UserName,
                                Estado = u.Estado ?? "Activo", // Manejo de nulos
                                Rol = r.Name ?? "Sin Rol"     // Asignamos el nombre del rol directo                                
                            };

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los usuarios: " + ex.InnerException.Message);
                throw new Exception("Error al bobtener los usuaios: " + ex.Message);
            }
        }

        public async Task<IList<string>> GetRolByUser(Usuario usuario)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                return roles;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los roles: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los roles: " + ex.Message);
            }
        }

        public async Task Create(Usuario usuario, string password, string rol) { 
            try
            {
                // Crear el usuario
                var result = await _userManager.CreateAsync(usuario, password);
                if (!result.Succeeded)
                {
                    var errores = string.Join("<br/>", result.Errors.Select(e => e.Description));
                    throw new Exception(errores);
                }

                // Asignar el rol
                await _userManager.AddToRoleAsync(usuario, rol);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Data Access: " + ex.InnerException.Message);
                throw new Exception("Data Access: " + ex.Message);
            }   
        }

        public async Task Update(Usuario usuario)
        {
            try
            {
                var result = await _userManager.UpdateAsync(usuario);
                if (!result.Succeeded)
                {
                    var errores = string.Join("<br/>", result.Errors.Select(e => e.Description));
                    throw new Exception(errores);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el usuario: " + ex.InnerException.Message);
                throw new Exception("Error al actualizar el usuario: " + ex.Message);
            }
        }

        public async Task CambiarRol(Usuario userView, string nuevoRol)
        {
            try
            {
                // Quitar roles actuales
                var rolesActuales = await _userManager.GetRolesAsync(userView);
                await _userManager.RemoveFromRolesAsync(userView, rolesActuales);

                // Agregar nuevo rol
                var result = await _userManager.AddToRoleAsync(userView, nuevoRol);                
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al cambiar el rol del usuario: " + ex.InnerException.Message);
                throw new Exception("Error al cambiar el rol del usuario: " + ex.Message);
            }
        }
    }
}
