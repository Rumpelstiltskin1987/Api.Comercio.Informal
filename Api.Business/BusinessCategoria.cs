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
        private readonly DataCategoriaLog _categoriaLog;

        public BusinessCategoria(MySQLiteContext context)
        {
            _context = context;
            _categoria = new(_context);
            _categoriaLog = new(_context);
        }

        public async Task<IEnumerable<Entities.Categoria>> GetAll()
        {
            return await _categoria.GetAll();
        }

        public async Task<Entities.Categoria> GetById(int id)
        {
            return await _categoria.GetById(id);
        }

        public async Task<bool> Create(Categoria categoria)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool result = await _categoria.Create(categoria);

                CategoriaLog log = new()
                {
                    Id_movimiento = 1,
                    Id_categoria = categoria.Id_categoria,
                    Nombre = categoria.Nombre,
                    Estado = categoria.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = categoria.Usuario_alta,
                    Fecha_modificacion = categoria.Fecha_alta
                };

                await _categoriaLog.AddLog(log);
                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> Update(int id, string nombre, string status, string usuario)
        {
            Categoria categoria = await _categoria.GetById(id);

            categoria.Nombre = nombre;
            categoria.Estado = status;
            categoria.Usuario_modificacion = usuario;
            categoria.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool result = await _categoria.Update(categoria);
                int idMovimiento = await _categoriaLog.GetIdMovement(id) + 1;

                CategoriaLog log = new()
                {
                    Id_movimiento = idMovimiento,
                    Id_categoria = categoria.Id_categoria,
                    Nombre = categoria.Nombre,
                    Estado = categoria.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = categoria.Usuario_modificacion,
                    Fecha_modificacion = categoria.Fecha_modificacion,
                };

                await _categoriaLog.AddLog(log);
                transaction.Commit();
                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            _ = await _categoria.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool result = await _categoria.Delete(id);
                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }        
    }
}
