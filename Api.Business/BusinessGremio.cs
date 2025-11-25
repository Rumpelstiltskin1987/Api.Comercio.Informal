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
    public class BusinessGremio : IGremio
    {
        private readonly MySQLiteContext _context;
        private readonly DataGremio _gremio;
        private readonly DataGremioLog _gremioLog;

        public BusinessGremio(MySQLiteContext context)
        {
            _context = context;
            _gremio = new DataGremio(_context);
            _gremioLog = new DataGremioLog(_context);
        }

        public async Task<IEnumerable<Gremio>> GetAll()
        {
            return await _gremio.GetAll();
        }

        
        public async Task<Gremio> GetById(int id)
        {
            return await _gremio.GetById(id);
        }

        public async Task<IEnumerable<Gremio>> Search(string? descripcion, int? id_lider, string? estado)
        {
            var query = _context.Gremio.AsQueryable();

            if (!string.IsNullOrEmpty(descripcion))
            {
                query = query.Where(g => g.Descripcion.Contains(descripcion));
            }

            if (id_lider.HasValue)
            {
                query = query.Where(g => g.Id_lider == id_lider);
            }

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(g => g.Estado == estado);
            }

            return await _gremio.Search(query);
        }

        public async Task Create(string descripcion, int id_lider, string usuario)
        {
            Gremio gremio = new Gremio
            {
                Descripcion = descripcion,
                Id_lider = id_lider,
                Usuario_alta = usuario,
                Fecha_alta = DateTime.Now
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _gremio.Create(gremio);

                GremioLog log = new()
                {
                    Id_movimiento = 1,
                    Id_gremio = gremio.Id_gremio,
                    Descripcion = gremio.Descripcion,
                    Id_lider = gremio.Id_lider,
                    Estado = gremio.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = gremio.Usuario_modificacion,
                    Fecha_modificacion = gremio.Fecha_modificacion
                };

                await _gremioLog.AddLog(log);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Update(int id, string descripcion, int id_lider, string status, string usuario)
        {
            Gremio gremio = await _gremio.GetById(id);

            gremio.Descripcion = descripcion;
            gremio.Id_lider = id_lider;
            gremio.Estado = status;
            gremio.Usuario_modificacion = usuario;
            gremio.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _gremio.Update(gremio);
                var idMovimiento = await _gremioLog.GetIdMovement(id) + 1;

                GremioLog log = new()
                {
                    Id_movimiento = idMovimiento,
                    Id_gremio = gremio.Id_gremio,
                    Descripcion = gremio.Descripcion,
                    Id_lider = gremio.Id_lider,
                    Estado = gremio.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = gremio.Usuario_modificacion,
                    Fecha_modificacion = gremio.Fecha_modificacion
                };

                await _gremioLog.AddLog(log);
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
            _ = await _gremio.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _gremio.Delete(id);
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
