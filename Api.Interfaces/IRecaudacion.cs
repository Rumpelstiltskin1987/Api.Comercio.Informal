using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    internal interface IRecaudacion
    {
        public Task<IEnumerable<Recaudacion>> GetAll();
        public Task<Lider> GetById(int id);
        public Task<bool> Create(Recaudacion recaudacion);
        public Task<bool> Update(int id, Recaudacion recaudacion);
        public Task<bool> Delete(int id);
    }
}
