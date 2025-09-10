using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface ICategoria
    {
        public Task<IEnumerable<Categoria>> GetAll();
        public Task<Categoria> GetById(int id);
        public Task<bool> Create(Categoria categoria);
        public Task<bool> Update(int id, string categoryName, string status);
        public Task<bool> Delete(int id);
    }
}
