using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business
{
    public class BusinessGremio : IGremio
    {
        private readonly MySQLiteContext _context;
        private readonly DataGremio _gremio;
        private readonly DataGremioLog _gremioLog;
        private readonly DataFolio _folio;
        private readonly DataLider _lider;

        public BusinessGremio(MySQLiteContext context)
        {
            _context = context;
            _gremio = new DataGremio(_context);
            _gremioLog = new DataGremioLog(_context);
            _folio = new DataFolio(_context);
            _lider = new DataLider(_context);
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

        public async Task Create(string descripcion, int id_lider, string prefijoManual, string usuario)
        {
            string prefijo = string.Empty;
            string prefijoCalculado = descripcion.Length >= 3
                ? descripcion[..3].ToUpper()
                : descripcion.ToUpper();

            bool existePrefijo = await _context.Gremio
                .AnyAsync(g => g.Prefijo == prefijoCalculado);

            if (existePrefijo && prefijoManual == null)
            {                
                throw new Exception($"El prefijo '{prefijoCalculado}' ya está ocupado por otro gremio. Por favor asigne un prefijo manual diferente.");
            }

            if (!string.IsNullOrEmpty(prefijoManual))
            {
                bool existePrefijoManual = await _context.Gremio
                    .AnyAsync(g => g.Prefijo == prefijoManual.ToUpper());
                if (existePrefijoManual)
                {
                    throw new Exception($"El prefijo manual '{prefijoManual}' ya está ocupado por otro gremio. Por favor asigne un prefijo manual diferente.");
                }
                prefijo = prefijoManual.ToUpper();
            }
            else
            {
                prefijo = prefijoCalculado;
            }

            Gremio gremio = new()
            {
                Descripcion = descripcion.ToUpper(),
                Id_lider = id_lider,
                Usuario_alta = usuario,
                Prefijo = prefijo
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _gremio.Create(gremio);
                var lider = await _lider.GetById(id_lider);

                GremioLog log = new()
                {
                    Id_movimiento = 1,
                    Id_gremio = gremio.Id_gremio,
                    Descripcion = gremio.Descripcion,
                    Lider = $"{lider.Nombre} {lider.A_paterno} {lider.A_materno}",
                    Prefijo = gremio.Prefijo,
                    Estado = gremio.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = gremio.Usuario_alta,
                    Fecha_modificacion = gremio.Fecha_alta
                };

                await _gremioLog.AddLog(log);

                Folio folio = new()
                {
                    Id_gremio = gremio.Id_gremio,
                    Descripcion = $"Folio correspondiente al gremio {gremio.Descripcion}",
                    Prefijo = gremio.Prefijo
                };

                await _folio.Create(folio);

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
            Folio folio = await _folio.GetByGremioId(id);

            if (folio.Siguiente_folio > 1)
            {
                throw new Exception("No se puede modificar el gremio porque ya se han generado lotes de folios asociados a él.");
            }

            Gremio gremio = await _gremio.GetById(id);

            gremio.Descripcion = descripcion;
            gremio.Id_lider = id_lider;
            gremio.Estado = status;
            gremio.Usuario_modificacion = usuario;
            gremio.Fecha_modificacion = DateTime.UtcNow;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _gremio.Update(gremio);
                var lider = await _lider.GetById(id_lider);
                var idMovimiento = await _gremioLog.GetIdMovement(id) + 1;

                GremioLog log = new()
                {
                    Id_movimiento = idMovimiento,
                    Id_gremio = gremio.Id_gremio,
                    Descripcion = gremio.Descripcion,
                    Lider = $"{lider.Nombre} {lider.A_paterno} {lider.A_materno}",
                    Prefijo = gremio.Prefijo,
                    Estado = gremio.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = gremio.Usuario_modificacion,
                    Fecha_modificacion = gremio.Fecha_modificacion
                };

                await _gremioLog.AddLog(log);

                //var query = _context.Folio.AsQueryable();
                //query = query.Where(f => f.Id_gremio == gremio.Id_gremio);

                //var folios = await _folio.Search(query);
                //Folio? folio = folios.FirstOrDefault();

                if (folio != null)
                {
                    folio.Descripcion = $"Folio correspondiente al gremio {gremio.Descripcion}";
                    folio.Prefijo = gremio.Descripcion.Length >= 3
                        ? gremio.Descripcion[..3].ToUpper()
                        : gremio.Descripcion.ToUpper();
                    await _folio.Update(folio);
                }

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

        public async Task<List<DtoHistorial>> GetHistorial(int id)
        {
            try
            {
                // 1. Obtenemos la lista cruda de la base de datos
                var logs = await _gremioLog.GetLogsByGremioId(id);

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
                    .AppendLine($"Lider: {log.Lider} | ")
                    .AppendLine($"Estado: {(log.Estado == "A" ? "Activo" : (log.Estado == "I" ? "Inactivo" : log.Estado))}")
                    .ToString()
                }).ToList();

                return historial;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el historial", ex);
            }
        }

        public async Task<IEnumerable<DtoGremio>> Sincronizar(DateTime? fSincronizacion)
        {
            IEnumerable<Gremio> listaDb;
            IEnumerable<DtoGremio> lista;

            if (fSincronizacion == null)
            {
                listaDb = await _gremio.GetAll();
            }
            else
            {
                listaDb = await _gremio.Sincronizar(fSincronizacion);
            }

            lista = listaDb.Select(p => new DtoGremio
            {
                IdGremio = p.Id_gremio,
                Descripcion = p.Descripcion ?? string.Empty,
            });

            return lista;
        }
    }
}
