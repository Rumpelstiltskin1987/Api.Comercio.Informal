using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IUsuario
    {
        public Task<IEnumerable<Usuario>> GetAll();
        public Task<Usuario> GetById(string id);

        public Task<IEnumerable<Usuario>> GetByRol(string rol);
    }
}
