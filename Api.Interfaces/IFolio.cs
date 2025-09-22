using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface IFolio
    {
        public Task<IEnumerable<Folio>> GetAll();
        public Task<Folio> GetById(int id);
        public Task<bool> Create(Folio folio);
        public Task<bool> Update(int id, Folio folio);
        public Task<bool> Delete(int id);
    }
}
