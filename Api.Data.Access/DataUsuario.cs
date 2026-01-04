using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access
{
    public class DataUsuario(UserManager<Usuario> userManager)
    {       

        public async Task<IEnumerable<Usuario>> GetAll()
        {
            IEnumerable<Usuario> usuarios;

            try
            {
                usuarios = await userManager.Users.ToListAsync();
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
                usuario = await userManager.FindByIdAsync(id) ?? throw new Exception("Usuario no encontrado");
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
                usuarios = await userManager.GetUsersInRoleAsync(rol) ?? throw new Exception("No se encontraron usuarios con el rol especificado");

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los registros: " + ex.InnerException.Message);
                throw new Exception("Error al buscar los registros: " + ex.Message);
            }

            return usuarios;
        }
    }
}
