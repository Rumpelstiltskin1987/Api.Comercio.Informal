using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task Create(string descripcion, string usuario)
        {
            Concepto concepto = new()
            {
                Descripcion = descripcion,
                Usuario_alta = usuario,
                Fecha_alta = DateTime.Now
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _concepto.Create(concepto);

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
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }              

        public async Task Update(int id, string descripcion, string estado, string usuario)
        {
            Concepto concepto = await _concepto.GetById(id);

            concepto.Descripcion = descripcion;
            concepto.Estado = estado;
            concepto.Usuario_modificacion = usuario;
            concepto.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            try 
            { 
                await _concepto.Update(concepto);
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
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            _ = await _concepto.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _concepto.Delete(id);
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
            try
            {
                // 1. Obtenemos la lista cruda de la base de datos
                var logs = await _conceptoLog.GetLogsByGremioId(id);

                // 2. Transformamos (Mapeamos) cada UsuarioLog a DtoHistorial
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
                    .AppendLine($"Descripcion: {log.Descripcion} | ")
                    .AppendLine($"Estado: {(log.Estado == "A" ? "Activo" : (log.Estado == "I" ? "Inactivo" : log.Estado))}")
                    .ToString()
                }).ToList();

                return historial;
            }
            catch (Exception ex)
            {
                // Es buena práctica loguear el error antes de lanzarlo, si tienes un logger
                throw new Exception("Error al obtener el historial", ex);
            }
        }
    }
}
