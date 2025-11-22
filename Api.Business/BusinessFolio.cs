using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Data.Access;
using Api.Entities;
using Api.Interfaces;

namespace Api.Business
{
    public class BusinessFolio : IFolio
    {
        private readonly MySQLiteContext _context;
        private readonly DataFolio _dataFolio;
        public BusinessFolio(MySQLiteContext context)
        {
            _context = context;
            _dataFolio = new DataFolio(_context);
        }

        public async Task<IEnumerable<Folio>> GetAll()
        {
            return await _dataFolio.GetAll();
        }

        public async Task<Folio> GetById(int id)
        {
            return await _dataFolio.GetById(id);
        }

        public async Task Create(int id_gremio, string descripcion, string prefijo)
        {
            Folio folio = new()
            {
                Id_gremio = id_gremio,
                Descripcion = descripcion,
                Prefijo = prefijo
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _dataFolio.Create(folio);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Update(int id, int id_gremio, string descripcion, string prefijo,
            int siguiente_folio, int anio_vigente)
        {
            Folio folio = await _dataFolio.GetById(id);
            
            folio.Descripcion = descripcion;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _dataFolio.Update(folio);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            _ = await _dataFolio.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _dataFolio.Delete(id);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
