using Api.Interfaces;
using Api.Entities;
using Api.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Api.Business
{
    public class BusinessUsuario : IUsuario
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly DataUsuario _dataUsuario;

        public BusinessUsuario(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
            _dataUsuario = new DataUsuario(_userManager);
        }
        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return await _dataUsuario.GetAll();
        }
        public async Task<Usuario> GetById(string id)
        {
            return await _dataUsuario.GetById(id);
        }
        public async Task<IEnumerable<Usuario>> GetByRol(string rol)
        {
            return await _dataUsuario.GetByRol(rol);
        }   
    }
}
