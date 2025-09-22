using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface ILider
    {
        public Task<IEnumerable<Lider>> GetAll();
        public Task<Lider> GetById(int id);
        public Task<bool> Create(Lider lider);
        public Task<bool> Update(int id, Lider lider);
        public Task<bool> Delete(int id);
    }
}
