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
        public Task<Lider> GetById(int id);
        public Task<bool> Create(Padron padron);
        public Task<bool> Update(int id, Padron padron);
        public Task<bool> Delete(int id);
    }
}
