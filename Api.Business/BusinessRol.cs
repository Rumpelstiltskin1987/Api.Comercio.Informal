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
    public class BusinessRol : IRol
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly DataRol _dataRol;

        public BusinessRol(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
            _dataRol = new DataRol(_roleManager);
        }
        public async Task<List<string>> GetAll()
        {
            return await _dataRol.GetAll();
        }
    }
}
