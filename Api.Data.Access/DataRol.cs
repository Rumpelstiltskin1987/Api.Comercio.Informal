using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access
{
    public class DataRol(RoleManager<IdentityRole<int>> roleManager)
    {
        public async Task<List<string>> GetAll()
        {
            try
            {
                var roles = await roleManager.Roles.Select(r => r.Name).ToListAsync(); ;
                return roles;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los roles: " + ex.InnerException.Message);
                throw new Exception("Error al obtener los roles: " + ex.Message);
            }
        }        
    }
}
