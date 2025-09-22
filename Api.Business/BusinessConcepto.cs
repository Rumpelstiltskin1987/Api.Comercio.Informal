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
    public class BusinessConcepto : IConcepto
    {
        private readonly MySQLiteContext _context;
        private readonly DataConcepto _concepto;
        private readonly DataConceptoLog _conceptoLog;

        public BusinessConcepto(MySQLiteContext context)
        {
            _context = context;
            _concepto = new(_context);
            _conceptoLog = new(_context);
        }

        public async Task<IEnumerable<Concepto>> GetAll()
        {
            return await _concepto.GetAll();
        }

        public async Task<Concepto> GetById(int id)
        {
            return await _concepto.GetById(id);
        }

        public async Task<bool> Create(Concepto concepto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                bool result = await _concepto.Create(concepto);

                ConceptoLog log = new()
                {
                    Id_movimiento = 1,
                    Id_concepto = concepto.Id_concepto,
                    Descripcion = concepto.Descripcion,
                    Estado = concepto.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = concepto.Usuario_alta,
                    Fecha_modificacion = concepto.Fecha_alta
                };
                await _conceptoLog.AddLog(log);
                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }              

        public async Task<bool> Update(int id, Concepto newConcepto)
        {
            Concepto concepto = await _concepto.GetById(id);

            concepto.Descripcion = newConcepto.Descripcion;
            concepto.Estado = newConcepto.Estado;
            concepto.Usuario_modificacion = newConcepto.Usuario_modificacion;
            concepto.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();

            try 
            { 
                bool result = await _concepto.Update(concepto);
                int id_movimiento = await _conceptoLog.GetIdMovement(id) + 1;

                ConceptoLog log = new()
                {
                    Id_movimiento = id_movimiento,
                    Id_concepto = concepto.Id_concepto,
                    Descripcion = concepto.Descripcion,
                    Estado = concepto.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = concepto.Usuario_modificacion,
                    Fecha_modificacion = concepto.Fecha_modificacion
                };

                await _conceptoLog.AddLog(log);
                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> Delete(int id, string usuario)
        {
            Concepto concepto = await _concepto.GetById(id);

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                bool result = await _concepto.Delete(id);
                int id_movimiento = await _conceptoLog.GetIdMovement(id) + 1;

                ConceptoLog log = new()
                {
                    Id_movimiento = id_movimiento,
                    Id_concepto = concepto.Id_concepto,
                    Descripcion = concepto.Descripcion,
                    Estado = concepto.Estado,
                    Tipo_movimiento = "B",
                    Usuario_modificacion = usuario,
                    Fecha_modificacion = DateTime.Now
                };

                await _conceptoLog.AddLog(log);
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
