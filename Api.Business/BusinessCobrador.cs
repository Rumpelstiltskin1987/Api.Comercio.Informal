using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Data.Access;
using Api.Entities;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Api.Business
{
    public class BusinessCobrador : ICobrador
    {
        private readonly MySQLiteContext _context;
        private readonly DataCobrador _cobrador;
        public BusinessCobrador(MySQLiteContext context)
        {
            _context = context;
            _cobrador = new(_context);
        }

        public async Task<bool> Create(Cobrador categoria)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool result = await _cobrador.Create(categoria);
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

        public async Task<bool> Delete(int id)
        {
            Cobrador categoria = await _cobrador.GetById(id);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool result = await _cobrador.Delete(id);
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

        public async Task<IEnumerable<Cobrador>> GetAll()
        {
            return await _cobrador.GetAll();
        }

        public async Task<Cobrador> GetById(int id)
        {
            return await _cobrador.GetById(id);
        }

        public async Task<bool> Update(int id, string nombre, string aPaterno, string aMaterno,
            string telefono, string email, string status, string usuario)
        {
            Cobrador cobrador = await _cobrador.GetById(id);

            if (cobrador == null)
                throw new Exception("El cobrador que intenta actualizar no existe en la base de datos.");

            cobrador.Nombre = nombre;
            cobrador.A_paterno = aPaterno;
            cobrador.A_paterno = aMaterno;
            cobrador.Telefono = telefono;
            cobrador.Email = email;
            cobrador.Estado = status;
            cobrador.Usuario_modificacion = usuario;
            cobrador.Fecha_modificacion = DateTime.Now;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    bool result = await _cobrador.Update(cobrador);
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
}
