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
        public Task<bool> CreateGremio(Gremio gremio);
        public Task<bool> UpdateGremio(int id, Gremio gremio);
        public Task<bool> DeleteGremio(int id);
    }
}
