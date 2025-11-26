using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface IPadron
    {
        public Task<IEnumerable<Padron>> GetAll();
        public Task<Padron> GetById(int id);
        public Task Create( string nombre, string a_paterno, string a_materno, string curp,
            string direccion, string telefono, string? email, int id_gremio, string tipo, string usuario);
        public Task Update(int id,  string nombre, string a_paterno, string a_materno, string curp,
            string direccion, string telefono, string email, string matricula,  string matricula_anterior, 
            int id_gremio, string status, string usuario);
        public Task Delete(int id);
    }
}
