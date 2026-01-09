using Api.Data;
using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business
{
    public class BusinessTarifa : IBusinessTarifa
    {
        private readonly MySQLiteContext _context;
        private readonly DataTarifa _tarifa;
        private readonly DataTarifaLog _tarifaLog;
        private readonly DataGremio _gremio;
        private readonly DataConcepto _concepto;

        public BusinessTarifa(MySQLiteContext context)
        {
            _context = context;
            _tarifa = new(_context);
            _tarifaLog = new(_context);
            _gremio = new(_context);
            _concepto = new(_context);
        }

        public async Task<IEnumerable<Tarifa>> GetAll()
        {
            return await _tarifa.GetAll();
        }

        public async Task<Tarifa> GetById(int id)
        {
            return await _tarifa.GetById(id);
        }

        public async Task<IEnumerable<Tarifa>> Search(int? id_concepto, int? id_gremio, decimal? monto, string? estado)
        {
            var query = _context.Tarifa.AsQueryable();
            if (id_concepto.HasValue)
            {
                query = query.Where(t => t.Id_concepto == (id_concepto));
            }
            if (id_gremio.HasValue)
            {
                query = query.Where(t => t.Id_gremio == id_gremio);
            }
            if (monto.HasValue)
            {
                query = query.Where(t => (decimal)t.Monto == monto.Value);
            }
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(t => t.Estado == estado);
            }
            return await query.ToListAsync();
        }

        public async Task Create(int id_concepto, int id_gremio, decimal? monto, string usuario)
        {
            Tarifa tarifa = new()
            {
                Id_concepto = id_concepto,
                Id_gremio = id_gremio,
                Monto = monto ?? 0,
                Usuario_alta = usuario
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _tarifa.Create(tarifa);
                var gremio = await _gremio.GetById(id_gremio);
                var concepto =  await _concepto.GetById(id_concepto);   

                TarifaLog log = new()
                {   
                    Id_movimiento = 1,
                    Id_tarifa = tarifa.Id_tarifa,
                    Concepto = concepto.Descripcion,
                    Gremio = gremio.Descripcion,
                    Monto = tarifa.Monto,
                    Estado = tarifa.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = tarifa.Usuario_alta,
                    Fecha_modificacion = tarifa.Fecha_alta
                };

                await _tarifaLog.AddLog(log);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Update(int id, int id_concepto, int id_gremio, decimal? monto, string estado,
            string usuario)
        {
            Tarifa tarifa = await _tarifa.GetById(id);

            tarifa.Id_concepto = id_concepto;
            tarifa.Id_gremio = id_gremio;
            tarifa.Monto = monto ?? 0;
            tarifa.Estado = estado;
            tarifa.Usuario_modificacion = usuario;
            tarifa.Fecha_modificacion = DateTime.Now;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _tarifa.Update(tarifa);
                int idMovimiento = await _tarifaLog.GetIdMovement(id) + 1;
                var gremio = await _gremio.GetById(id_gremio);
                var concepto = await _concepto.GetById(id_concepto);

                TarifaLog log = new()
                {
                    Id_movimiento = idMovimiento,
                    Id_tarifa = tarifa.Id_tarifa,
                    Concepto = concepto.Descripcion,
                    Gremio = gremio.Descripcion,
                    Monto = tarifa.Monto,
                    Estado = tarifa.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = tarifa.Usuario_alta,
                    Fecha_modificacion = tarifa.Fecha_alta
                };

                await _tarifaLog.AddLog(log);
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
            _ = await _tarifa.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _tarifa.Delete(id);
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
                var logs = await _tarifaLog.GetLogsByTarifaId(id);

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
                    .AppendLine($"Concepto: {log.Concepto} | ")
                    .AppendLine($"Gremio: {log.Gremio} | ")
                    .AppendLine($"Monto: {log.Monto} | ")
                    .AppendLine($"Estado: {(log.Estado == "A" ? "Activo" : (log.Estado == "I" ? "Inactivo" : log.Estado))}")
                    .ToString()
                }).ToList();

                return historial;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
