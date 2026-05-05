using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api.Business
{
    public class BusinessSolicitudCancelacion
    {
        private readonly MySQLiteContext _context;
        private readonly DataSolicitudCancelacion _solicitudCancelacion;
        private readonly DataRecaudacion _recaudacion;

        public BusinessSolicitudCancelacion(MySQLiteContext context)
        {
            _context = context;
            _solicitudCancelacion = new DataSolicitudCancelacion(_context);
            _recaudacion = new DataRecaudacion(_context);
        }

        public async Task<IEnumerable<SolicitudCancelacion>> GetAll()
        {
            return await _solicitudCancelacion.GetAll();
        }

        public async Task<SolicitudCancelacion?> GetById(int id)
        {
            return await _solicitudCancelacion.GetById(id);
        }

        public async Task<IEnumerable<DtoBandejaSolicitud>> GetPending()
        {
            var solicitudes = await _solicitudCancelacion.GetAll();

            var filtrados = solicitudes.Where(s => s.Estado_solicitud == "P");

            return filtrados.Select(s => new DtoBandejaSolicitud
            {
                Id_solicidud = s.Id_solicitud,
                Folio_recibo = s.Recaudacion?.Folio_Recibo ?? string.Empty,
                Concepto = s.Recaudacion?.Concepto?.Descripcion ?? string.Empty,
                Monto = s.Recaudacion?.Monto ?? 0,
                Nombre_cobrador = s.UsuarioSolicita != null
                ? $"{s.UsuarioSolicita.Nombre} {s.UsuarioSolicita.A_paterno} {s.UsuarioSolicita.A_materno}"
                : "Usuario no encontrado",
                Fecha_solicitud = s.Fecha_solicitud,
                Motivo_solicitud = s.Motivo_solicitud,
                Estado_solicitud = s.Estado_solicitud
            });
        }

        public async Task<IEnumerable<DtoBandejaSolicitud>> GetHistory()
        {
            var solicitudes = await _solicitudCancelacion.GetAll();

            var filtrados = solicitudes.Where(s => s.Estado_solicitud != "P");

            return filtrados.Select(s => new DtoBandejaSolicitud
            {
                Id_solicidud = s.Id_solicitud,
                Folio_recibo = s.Recaudacion?.Folio_Recibo ?? string.Empty,
                Concepto = s.Recaudacion?.Concepto?.Descripcion ?? string.Empty,
                Monto = s.Recaudacion?.Monto ?? 0,
                Nombre_cobrador = s.UsuarioSolicita != null
                ? $"{s.UsuarioSolicita.Nombre} {s.UsuarioSolicita.A_paterno} {s.UsuarioSolicita.A_materno}"
                : "Usuario no encontrado",
                Fecha_solicitud = s.Fecha_solicitud,
                Motivo_solicitud = s.Motivo_solicitud,
                Estado_solicitud = s.Estado_solicitud
            });
        }

        public async Task<IEnumerable<SolicitudCancelacion>> Search(int idUsuarioSolicita, DateTime? fechaSolicitud, 
            string? motivoSolicitud, string? estado, int idUsuarioResponde, DateTime? fechaRespuesta, 
            string? motivoRespuesta )
        {
            var query = _context.SolicitudCancelacion.AsQueryable();

            if (idUsuarioSolicita > 0)
            {
                query = query.Where(s => s.Id_usuario_solicita == idUsuarioSolicita);
            }

            if (fechaSolicitud.HasValue)
            {
                query = query.Where(s => s.Fecha_solicitud.Date == fechaSolicitud.Value.Date);
            }

            if (!string.IsNullOrEmpty(motivoSolicitud))
            {
                query = query.Where(s => s.Motivo_solicitud.Contains(motivoSolicitud));
            }
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(s => s.Estado_solicitud == estado);
            }

            if (idUsuarioResponde > 0)
            {
                query = query.Where(s => s.Id_usuario_responde == idUsuarioResponde);
            }

            if (fechaRespuesta.HasValue)
            {
                query = query.Where(s => s.Fecha_respuesta.HasValue && s.Fecha_respuesta.Value.Date == fechaRespuesta.Value.Date);
            }

            if (!string.IsNullOrEmpty(motivoRespuesta))
            {
                query = query.Where(s => s.Motivo_respuesta != null && s.Motivo_respuesta.Contains(motivoRespuesta));
            }

            return await _solicitudCancelacion.Search(query);
        }

        public async Task UpdateSolicitud(DtoBandejaSolicitud solicitud, DtoProcesarSolicitud respuesta)
        {
            var solicitudExistente = await _solicitudCancelacion.GetById(solicitud.Id_solicidud) 
                ?? throw new InvalidOperationException("La solicitud de cancelación no existe.");

            solicitudExistente.Estado_solicitud = solicitud.Estado_solicitud;            
            solicitudExistente.Motivo_respuesta = respuesta.Motivo_respuesta;
            solicitudExistente.Id_usuario_responde = respuesta.Id_usuario_responde;
            solicitudExistente.Fecha_respuesta = DateTime.UtcNow;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try {
                await _solicitudCancelacion.UpdateSolicitud(solicitudExistente);

                if (solicitud.Estado_solicitud == "A")
                {
                    var recaudacion = await _recaudacion.GetById(solicitudExistente.Id_recaudacion);
                    if (recaudacion != null)
                    {
                        recaudacion.Estado = "C";
                        await _recaudacion.Update(recaudacion);
                    }
                }
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
