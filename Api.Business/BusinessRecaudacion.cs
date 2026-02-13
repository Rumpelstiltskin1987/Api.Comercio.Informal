using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Business
{
    public class BusinessRecaudacion : IRecaudacion
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly MySQLiteContext _context;
        private readonly DataRecaudacion _recaudacion;
        private readonly DataFolio _folio;
        private readonly DataLoteFolio _lote;
        private readonly DataSolicitudCancelacion _solicitudCancelacion;
        private readonly DataUsuario _usuario;

        public BusinessRecaudacion(MySQLiteContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
            _recaudacion = new(_context);
            _folio = new(_context);
            _lote = new(_context);
            _solicitudCancelacion = new(_context);
            _usuario = new(_userManager, _context);
        }

        public async Task<IEnumerable<Recaudacion>> GetAll()
        {
            return await _recaudacion.GetAll();
        }

        public async Task<Recaudacion> GetById(int id)
        {
            return await _recaudacion.GetById(id);
        }

        public async Task<Recaudacion> GetByFolio(string folio)
        {
            return await _recaudacion.GetByFolio(folio);
        }

        public async Task<DtoRecaudacionDetalle> GetFolioDetail(string folio)
        {
            DtoRecaudacionDetalle detalle;
            try
            {
                var recaudacion = await _recaudacion.GetByFolio(folio);

                detalle = new()
                {
                    Id = recaudacion.Id_recaudacion,
                    FolioRecibo = recaudacion.Folio_Recibo,
                    NombreContribuyente = $"{recaudacion.Padron?.Nombre} {recaudacion.Padron?.A_paterno} {recaudacion.Padron?.A_materno}".Trim(),
                    MatriculaContribuyente = recaudacion.Padron?.Matricula ?? string.Empty,
                    GremioContribuyente = recaudacion.Padron?.Gremio?.Descripcion ?? string.Empty,
                    Concepto = recaudacion.Concepto?.Descripcion ?? string.Empty,
                    Monto = recaudacion.Monto,
                    FechaCobro = recaudacion.Fecha_cobro,
                    NombreCobrador = recaudacion.Cobrador?.UserName ?? "DESCONOCIDO",
                    Estado = recaudacion.Estado,
                };
                return detalle;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Recaudacion>> Search(int? idCobrador, int? idConcepto, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Recaudacion.AsQueryable();

            // Filtro por Cobrador
            if (idCobrador.HasValue && idCobrador > 0)
            {
                query = query.Where(c => c.Id_cobrador == idCobrador.Value);
            }

            // Filtro por Concepto
            if (idConcepto.HasValue && idConcepto > 0)
            {
                query = query.Where(c => c.Id_concepto == idConcepto.Value);
            }

            // Filtro por Fechas
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                // Usamos operadores estándar >= y <= porque '.between' no existe en C# LINQ
                query = query.Where(c => c.Fecha_cobro >= fechaInicio.Value && c.Fecha_cobro <= fechaFin);
            }

            query = query.OrderBy(c => c.Fecha_cobro);

            return await _recaudacion.Search(query);
        }

        public async Task Create(DtoRecaudacionCrear cobroRequest)
        {

            //var queryFolio = _context.Folio.AsQueryable().Where(f => f.Id_gremio == id_gremio);
            //var listaFolios = await _folio.Search(queryFolio);
            //var folioEncontrado = listaFolios.FirstOrDefault() ?? throw new Exception("No se encontró configuración de folios para el gremio especificado.");

            //if (folioEncontrado.Anio_vigente != DateTime.Now.Year)
            //{
            //    folioEncontrado.Anio_vigente = DateTime.Now.Year;
            //    folioEncontrado.Siguiente_folio = 1;
            //}
            // folioEncontrado.Siguiente_folio += 1;
            // await _folio.Update(folioEncontrado);

            //string folioRecibo = $"{folioEncontrado.Prefijo}{folioEncontrado.Anio_vigente % 100}{folioEncontrado.Siguiente_folio:D6}";

            var query = _context.LoteFolio.AsQueryable().Where(lf => lf.Id_usuario == cobroRequest.IdCobrador && lf.Id_gremio == cobroRequest.IdGremio);
            var listaLotes = await _lote.Search(query);
            var lote = listaLotes.FirstOrDefault();


            Recaudacion cobro = new()
            {
                Id_padron = cobroRequest.IdPadron,
                Id_concepto = cobroRequest.IdConcepto,
                Monto = cobroRequest.Monto,
                Id_cobrador = cobroRequest.IdCobrador,
                Fecha_cobro = cobroRequest.FechaCobro,
                Folio_Recibo = cobroRequest.FolioRecibo,
                Latitud = cobroRequest.Latitud,
                Longitud = cobroRequest.Longitud,
                Fecha_Alta = DateTime.UtcNow
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Validar que el folio no sea nulo y tenga la longitud mínima necesaria (5 + 6 = 11 caracteres)
                if (!string.IsNullOrEmpty(cobroRequest.FolioRecibo) && cobroRequest.FolioRecibo.Length >= 11)
                {
                    // Validamos que lote no sea null
                    if (lote == null)
                        throw new Exception("Ocurrió un error al buscar el lote de folio cobrado en la base de datos.");

                    // Extraemos la cadena
                    string parteNumerica = cobroRequest.FolioRecibo.Substring(5, 6);

                    // Intentamos convertir de forma segura (TryParse)
                    if (int.TryParse(parteNumerica, out int numeroFolio))
                    {
                        lote.Ultimo_usado = numeroFolio;
                        lote.Fecha_modificacion = DateTime.UtcNow;
                    }
                    else
                    {
                        // Manejar el error: La parte extraída no eran números
                        throw new Exception($"El folio '{cobroRequest.FolioRecibo}' no contiene un número válido en la posición esperada.");
                    }
                }
                else
                {
                    // Manejar el error: El folio es muy corto o nulo
                    throw new Exception("El formato del Folio Recibo es inválido o demasiado corto.");
                }                

                await _recaudacion.Create(cobro);
                await _lote.Update(lote);
                

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        Task IRecaudacion.Update(int id, Recaudacion recaudacion)
        {
            throw new NotImplementedException();
        }

        Task IRecaudacion.Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddSolicitudCancelacion(DtoCrearSolicitud solicitud)
        {
            // Validar que la recaudación exista antes de agregar la solicitud de cancelación
            _ = await _recaudacion.GetById(solicitud.IdRecaudacion) ?? throw new Exception("La recaudación asociada no existe.");
            _ = await _usuario.GetById(solicitud.IdUsuarioSolicita.ToString()) ?? throw new Exception("El usuario solicitante no existe.");
            SolicitudCancelacion nuevaSolicitud = new()
            {
                Id_recaudacion = solicitud.IdRecaudacion,
                Id_usuario_solicita = solicitud.IdUsuarioSolicita,               
                Fecha_solicitud = DateTime.UtcNow,
                Motivo_solicitud = solicitud.MotivoSolicitud,
                Estado_solicitud = "P" // P = Pendiente
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _solicitudCancelacion.AddSolicitud(nuevaSolicitud);
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
