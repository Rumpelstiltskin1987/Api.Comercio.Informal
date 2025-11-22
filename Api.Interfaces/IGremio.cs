using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface IGremio
    {
        public Task<IEnumerable<Gremio>> GetAll();
        public Task<Gremio> GetById(int id);
        public Task Create(string descripcion, int id_lider, string usuario);
        public Task Update(int id, string descripcion, int id_lider, string status, string usuario);
        public Task Delete(int id);
    }
}
