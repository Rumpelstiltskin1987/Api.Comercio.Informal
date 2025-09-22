using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface ICobrador
    {
        public Task<IEnumerable<Cobrador>> GetAll();
        public Task<Cobrador> GetById(int id);
        public Task<bool> Create(Cobrador categoria);
        public Task<bool> Update(int id, string nombre, string aPaterno, string aMaterno,
            string telefono, string email, string estado, string usuario);
        public Task<bool> Delete(int id);
    }
}
