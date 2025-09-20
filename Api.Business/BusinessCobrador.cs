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
        private readonly DataCobradorLog _cobradorLog;
        public BusinessCobrador(MySQLiteContext context)
        {
            _context = context;
            _cobrador = new(_context);
            _cobradorLog = new(_context);
        }

        public async Task<IEnumerable<Cobrador>> GetAll()
        {
            return await _cobrador.GetAll();
        }

        public async Task<Cobrador> GetById(int id)
        {
            return await _cobrador.GetById(id);
        }

        public async Task<bool> Create(Cobrador categoria)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool result = await _cobrador.Create(categoria);

                CobradorLog log = new()
                {
                    Id_movimiento = 1,
                    Id_cobrador = categoria.Id_cobrador,
                    Nombre = categoria.Nombre,
                    A_paterno = categoria.A_paterno,
                    A_materno = categoria.A_materno,
                    Telefono = categoria.Telefono,
                    Email = categoria.Email,
                    Estado = categoria.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = categoria.Usuario_alta,
                    Fecha_modificacion = categoria.Fecha_alta
                };

                await _cobradorLog.AddLog(log);

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
            _ = await _cobrador.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
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

        public async Task<bool> Update(int id, string nombre, string aPaterno, string aMaterno,
            string telefono, string email, string status, string usuario)
        {
            Cobrador cobrador = await _cobrador.GetById(id);

            cobrador.Nombre = nombre;
            cobrador.A_paterno = aPaterno;
            cobrador.A_materno = aMaterno;
            cobrador.Telefono = telefono;
            cobrador.Email = email;
            cobrador.Estado = status;
            cobrador.Usuario_modificacion = usuario;
            cobrador.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool result = await _cobrador.Update(cobrador);
                int idMovimiento = await _cobradorLog.GetIdMovement(id) + 1;

                CobradorLog log = new()
                {
                    Id_movimiento = idMovimiento,
                    Id_cobrador = cobrador.Id_cobrador,
                    Nombre = cobrador.Nombre,
                    A_paterno = cobrador.A_paterno,
                    A_materno = cobrador.A_materno,
                    Telefono = cobrador.Telefono,
                    Email = cobrador.Email,
                    Estado = cobrador.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = usuario,
                    Fecha_modificacion = DateTime.Now
                };

                await _cobradorLog.AddLog(log);
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
