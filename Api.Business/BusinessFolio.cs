using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business
{
    public class BusinessFolio : IFolio
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly MySQLiteContext _context;
        private readonly DataFolio _folio;
        private readonly DataGremio _gremio;
        private readonly DataLoteFolio _loteFolio;
        private readonly DataUsuario _usuario;
        public BusinessFolio(UserManager<Usuario> userManager, MySQLiteContext context)
        {
            _context = context;
            _userManager = userManager;
            _folio = new DataFolio(_context);
            _gremio = new DataGremio(_context);
            _loteFolio = new DataLoteFolio(_context);
            _usuario = new DataUsuario(_userManager, _context);
        }

        public async Task<IEnumerable<Folio>> GetAll()
        {
            return await _folio.GetAll();
        }

        public async Task<Folio> GetById(int id)
        {
            return await _folio.GetById(id);
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
                await _folio.Create(folio);
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
            Folio folio = await _folio.GetById(id);

            folio.Descripcion = descripcion;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _folio.Update(folio);
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
            _ = await _folio.GetById(id);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _folio.Delete(id);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<DtoLoteFolio>> SolicitarLotes(int idUsuario)
        {
            List<DtoLoteFolio> listadoLotes = [];
            var usuario = _usuario.GetById(idUsuario.ToString()).Result;
           

            // Traemos la lista de gremios
            var gremios = (await _gremio.GetAll()).ToList();

            // Iteramos para reservar folios para cada gremio
            foreach (var g in gremios)
            {
                // Obtener el folio del gremio para tener los datos disponibles
                var folio = _folio.GetByGremioId(g.Id_gremio).Result;

                // Verificar si el usuario ya tiene un lote ACTIVO y con folios disponibles
                var query = _context.LoteFolio.AsQueryable()
                    .Where(l => l.Id_usuario == idUsuario
                    && l.Id_gremio == g.Id_gremio
                    && l.Estado == "ACTIVO");

                var loteActivo = _loteFolio.Search(query)
                    .Result.FirstOrDefault();

                // Si aún tiene folios, le devolvemos el mismo lote
                if (loteActivo != null)
                {
                    // Generamos el lote (Modelo Dto que es el que se devolverá como respuesta)
                    DtoLoteFolio lote = new()
                    {
                        IdUsuario = idUsuario,
                        IdGremio = loteActivo.Id_gremio,
                        Prefijo = folio.Prefijo ?? string.Empty, 
                        Anio = folio.Anio_vigente,
                        FolioInicial = loteActivo.Rango_inicial,
                        FolioFinal = loteActivo.Rango_final,
                        UltimoUsado = loteActivo.Ultimo_usado
                    };

                    // Añadimos el lote a la lista
                    listadoLotes.Add(lote);
                }
                else
                {
                    // 2. Si no tiene, GENERAMOS UN NUEVO LOTE
                    using var transaction = _context.Database.BeginTransaction();
                    try
                    {
                        // Variables necesarias para la generación de los lotes                        
                        int inicio = folio.Siguiente_folio;
                        int cantidadLote = folio.Cantidad_lote; 
                        int fin = inicio + cantidadLote - 1;

                        // Actualizamos el contador global para que el siguiente usuario empiece DESPUÉS de este lote
                        folio.Siguiente_folio = fin + 1;

                        // Generamos el registro de asignación de lote (modelo db)
                        LoteFolio nuevoLote = new()
                        {
                            Id_usuario = idUsuario,
                            Id_gremio = g.Id_gremio,
                            Rango_inicial = inicio,
                            Rango_final = fin,
                            Ultimo_usado = inicio - 1, // Aún no ha usado ninguno
                            Anio = folio.Anio_vigente
                        };

                        // Generamos el lote (Modelo Dto que es el que se devolverá como respuesta)
                        DtoLoteFolio loteDto = new()
                        {
                            IdUsuario = idUsuario,
                            IdGremio = nuevoLote.Id_gremio,
                            Prefijo = folio.Prefijo ?? string.Empty,
                            Anio = folio.Anio_vigente,
                            FolioInicial = nuevoLote.Rango_inicial,
                            FolioFinal = nuevoLote.Rango_final,
                            UltimoUsado = nuevoLote.Ultimo_usado
                        };
                        // Creamos el lote
                        await _loteFolio.Create(nuevoLote);
                        // Actualiziamos el folio
                        await _folio.Update(folio);
                        transaction.Commit();
                        // Añadimos el lote a la lista
                        listadoLotes.Add(loteDto);
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return listadoLotes;
        }


        public async Task<bool> AgotarLote(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var lote = await _loteFolio.GetById(id);
                lote.Estado = "AGOTADO";
                await _loteFolio.Update(lote);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
