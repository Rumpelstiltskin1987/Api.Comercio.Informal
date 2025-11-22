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

        public async Task<IEnumerable<Cobrador>> Search(string? nombre, string? aPaterno, string? aMaterno,
            string? telefono, string? email, string? estado)
        {
            var query = _context.Cobrador.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(c => c.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(aPaterno))
            {
                query = query.Where(c => c.A_paterno.Contains(aPaterno));
            }

            if (!string.IsNullOrEmpty(aMaterno))
            {
                query = query.Where(c => c.A_materno.Contains(aMaterno));
            }

            if (!string.IsNullOrEmpty(telefono))
            {
                query = query.Where(c => c.Telefono != null && c.Telefono.Contains(telefono));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(c => c.Email != null && c.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(c => c.Estado == estado);
            }

            return await _cobrador.Search(query);
        }

        public async Task Create(string nombre, string aPaterno, string aMaterno,
            string telefono, string? email, string usuario)
        {
            Cobrador cobrador = new()
            {
                Nombre = nombre,
                A_paterno = aPaterno,
                A_materno = aMaterno,
                Telefono = telefono,
                Email = email,
                Usuario_alta = usuario
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _cobrador.Create(cobrador);

                CobradorLog log = new()
                {
                    Id_movimiento = 1,
                    Id_cobrador = cobrador.Id_cobrador,
                    Nombre = cobrador.Nombre,
                    A_paterno = cobrador.A_paterno,
                    A_materno = cobrador.A_materno,
                    Telefono = cobrador.Telefono,
                    Email = cobrador.Email,
                    Estado = cobrador.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = cobrador.Usuario_alta,
                    Fecha_modificacion = cobrador.Fecha_alta
                };

                await _cobradorLog.AddLog(log);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }               

        public async Task Update(int id, string nombre, string aPaterno, string aMaterno,
            string telefono, string? email, string estado, string usuario)
        {
            Cobrador cobrador = await _cobrador.GetById(id);

            cobrador.Nombre = nombre;
            cobrador.A_paterno = aPaterno;
            cobrador.A_materno = aMaterno;
            cobrador.Telefono = telefono;
            cobrador.Email = email;
            cobrador.Estado = estado;
            cobrador.Usuario_modificacion = usuario;
            cobrador.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _cobrador.Update(cobrador);
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
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }        

        public async Task Delete(int id)
        {
            _ = await _cobrador.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _cobrador.Delete(id);
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
