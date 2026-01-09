using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;  


namespace Api.Business
{
    public class BusinessLider : ILider
    {
        private readonly MySQLiteContext _context;
        private readonly DataLider _lider;
        private readonly DataLiderLog _liderLog;

        public BusinessLider(MySQLiteContext context)
        {
            _context = context;
            _lider = new DataLider(_context);
            _liderLog = new DataLiderLog(_context);
        }

        public async Task<IEnumerable<Lider>> GetAll()
        {
            return await _lider.GetAll();
        }

        public async Task<Lider> GetById(int id)
        {
            return await _lider.GetById(id);
        }

        public async Task Create(string nombre, string a_paterno, string a_materno, string telefono, 
            string email, string direccion, string usuario)
        {
            var lider = new Lider
            {
                Nombre = nombre,
                A_paterno = a_paterno,
                A_materno = a_materno,
                Telefono = telefono,
                Email = email,
                Direccion = direccion,
                Usuario_alta = usuario
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _lider.Create(lider);

                LiderLog log = new()
                {
                    Id_movimiento = 1,
                    Id_lider = lider.Id_lider,
                    Nombre = lider.Nombre,
                    A_paterno = lider.A_paterno,
                    A_materno = lider.A_materno,
                    Telefono = lider.Telefono,
                    Email = lider.Email,
                    Direccion = lider.Direccion,
                    Estado = lider.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = usuario,
                    Fecha_modificacion = DateTime.Now
                };

                await _liderLog.AddLog(log);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Update(int id, string nombre, string a_paterno, string a_materno, string telefono, 
            string email, string direccion, string estado, string usuario)
        {
            Lider lider = await _lider.GetById(id);

            lider.Nombre = nombre;
            lider.A_paterno = a_paterno;
            lider.A_materno = a_materno;
            lider.Telefono = telefono;
            lider.Email = email;
            lider.Direccion = direccion;
            lider.Estado = estado;
            lider.Usuario_modificacion = usuario;
            lider.Fecha_modificacion = DateTime.Now;            
            
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _lider.Update(lider);
                var idmovimiento = await _liderLog.GetIdMovement(id) + 1;

                LiderLog log = new()
                {
                    Id_movimiento = idmovimiento,
                    Id_lider = lider.Id_lider,
                    Nombre = lider.Nombre,
                    A_paterno = lider.A_paterno,
                    A_materno = lider.A_materno,
                    Telefono = lider.Telefono,
                    Email = lider.Email,
                    Direccion = lider.Direccion,
                    Estado = lider.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = usuario,
                    Fecha_modificacion = DateTime.Now
                };

                await _liderLog.AddLog(log);
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
            _ = await _lider.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _lider.Delete(id);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        
        public async Task<List<DtoHistorial>> GetHistorial(int id)
        {
            // 1. Obtenemos la lista cruda de la base de datos
            var logs = await _liderLog.GetLogsByLiderId(id);

            // 2. Transformamos (Mapeamos) cada LiderLog a DtoHistorial
            var historial = logs.Select(log => new DtoHistorial
            {
                Fecha = log.Fecha_modificacion,
                Usuario = log.Usuario_modificacion,
                Movimiento = log.Tipo_movimiento.ToUpper() switch
                {
                    "A" => "Alta",
                    "M" => "Modificación",
                    _ => log.Tipo_movimiento
                },
                Detalles = new StringBuilder()
                    .AppendLine($"Nombre: {log.Nombre} | ")
                    .AppendLine($"A_paterno: {log.A_paterno} | ")
                    .AppendLine($"A_materno: {log.A_materno} | ")
                    .AppendLine($"Telefono: {log.Telefono} |")
                    .AppendLine($"Email: {log.Email} |")
                    .AppendLine($"Direccion: {log.Direccion} |")
                    .AppendLine($"Estado: {(log.Estado == "A" ? "Activo" : (log.Estado == "I" ? "Inactivo" : log.Estado))}")
                    .ToString()
            }).ToList();

            return historial;
        }
    }
}
