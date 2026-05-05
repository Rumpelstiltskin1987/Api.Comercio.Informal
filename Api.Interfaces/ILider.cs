using Api.Entities;
using Api.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface ILider
    {
        public Task<IEnumerable<Lider>> GetAll();
        public Task<Lider> GetById(int id);
        public Task Create(string nombre, string a_paterno, string a_materno, string telefono, 
            string email, string direccion, string usuario);
        public Task Update(int id, string nombre, string a_paterno, string a_materno, string telefono, 
            string email, string direccion, string estado, string usuario);
        public Task Delete(int id);
        public Task<List<DtoHistorial>> GetHistorial(int id);
    }
}
