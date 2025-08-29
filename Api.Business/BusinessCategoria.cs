using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Interfaces;
using Api.Data.Access;
namespace Api.Business
{
    public class BusinessCategoria : ICategoria
    {
        private readonly MySQLiteContext _context;
        private readonly DataCategoria _categoria;

        public BusinessCategoria(MySQLiteContext context)
        {
            _context = context;
            _categoria = new (_context);
        }

        public async Task<bool> Create(Categoria categoria)
        {
            return await _categoria.Create(categoria);
        }

        public async Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Categoria>> GetAll()
        {
            return await _categoria.GetAll();
        }

        public async Task<Categoria> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Categoria> Update(Categoria categoria)
        {
            throw new NotImplementedException();
            return false;
        }
    }
}
