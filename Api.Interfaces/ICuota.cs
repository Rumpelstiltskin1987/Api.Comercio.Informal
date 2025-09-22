using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface ICuota
    {
        public Task<IEnumerable<Cuota>> GetAll();
        public Task<Cuota> GetById(int id);
        public Task<bool> Create(Cuota cuota);
        public Task<bool> Update(decimal monto, string estado, string usuario);
        public Task<bool> Delete(int id);
    }
}
