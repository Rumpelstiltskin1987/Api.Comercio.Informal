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
        public Task<bool> Create(Concepto concepto);
        public Task<bool> Update(int id, Concepto newConcepto);
        public Task<bool> Delete(int id, string usuario);
    }
}
