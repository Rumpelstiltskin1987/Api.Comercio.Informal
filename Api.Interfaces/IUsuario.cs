using Api.Entities;
using Api.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IUsuario
    {
        public Task<IEnumerable<DtoUsuario>> GetAll();
        public Task<Usuario> GetById(string id);
        public Task<IEnumerable<Usuario>> GetByRol(string rol);
        public Task Create(DtoUsuario usuario);
        public Task Update(DtoUsuario usuario);
        public Task<List<DtoHistorial>> GetHistorial(int id);
    }
}
