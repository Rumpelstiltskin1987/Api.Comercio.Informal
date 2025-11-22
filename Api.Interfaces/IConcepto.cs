using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Interfaces
{
    public interface IConcepto
    {
        public Task<IEnumerable<Concepto>> GetAll();
        public Task<Concepto> GetById(int id);
        public Task Create(string descripcion, string usuario);
        public Task Update(int id, string descripcion, string estado, string usuario);
        public Task Delete(int id);
    }
}
