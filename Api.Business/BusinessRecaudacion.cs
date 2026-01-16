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
        private readonly DataSolicitudCancelacion _solicitudCancelacion;
        private readonly DataUsuario _usuario;

        public BusinessRecaudacion(MySQLiteContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
            _recaudacion = new(_context);
            _folio = new(_context);
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
                    NombreCobrador = $"{recaudacion.Cobrador?.Nombre} {recaudacion.Cobrador?.A_paterno} {recaudacion.Cobrador?.A_paterno}".Trim(),
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

            // Filtro por Concepto (Si es null o 0, lo ignora y trae todos)
            if (idConcepto.HasValue && idConcepto > 0)
            {
                query = query.Where(c => c.Id_concepto == idConcepto.Value);
            }

            // Filtro por Fechas (Corrigiendo el error de sintaxis y lógica)
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                // Ajustamos la fecha fin para incluir todo el día hasta las 23:59:59
                DateTime fechaFinAjustada = fechaFin.Value.Date.AddDays(1).AddTicks(-1);

                // Usamos operadores estándar >= y <= porque '.between' no existe en C# LINQ
                query = query.Where(c => c.Fecha_cobro >= fechaInicio.Value && c.Fecha_cobro <= fechaFinAjustada);
            }

            query = query.OrderByDescending(c => c.Fecha_cobro);

            return await _recaudacion.Search(query);
        }

        public async Task Create(int id_padron, int id_gremio, int id_concepto, decimal monto,
            int id_cobrador, double? latitud, double? longitud, DateTime fechaCobro)
        {

            var queryFolio = _context.Folio.AsQueryable().Where(f => f.Id_gremio == id_gremio);
            var listaFolios = await _folio.Search(queryFolio);
            var folioEncontrado = listaFolios.FirstOrDefault() ?? throw new Exception("No se encontró configuración de folios para el gremio especificado.");

            if (folioEncontrado.Anio_vigente != DateTime.Now.Year)
            {
                folioEncontrado.Anio_vigente = DateTime.Now.Year;
                folioEncontrado.Siguiente_folio = 1;
            }

            string folioRecibo = $"{folioEncontrado.Prefijo}{folioEncontrado.Anio_vigente % 100}{folioEncontrado.Siguiente_folio:D6}";

            Recaudacion cobro = new()
            {
                Id_padron = id_padron,
                Id_concepto = id_concepto,
                Monto = monto,
                Id_cobrador = id_cobrador,
                Fecha_cobro = fechaCobro,
                Folio_Recibo = folioRecibo,
                Latitud = latitud,
                Longitud = longitud,
                Fecha_Alta = DateTime.UtcNow
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (folioEncontrado.Siguiente_folio == 1)
                {
                    await _folio.Update(folioEncontrado);
                }

                folioEncontrado.Siguiente_folio += 1;

                await _recaudacion.Create(cobro);
                await _folio.Update(folioEncontrado);

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
