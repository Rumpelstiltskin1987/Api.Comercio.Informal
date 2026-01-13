using Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access
{
    public class DataSolicitudCancelacion(MySQLiteContext context)
    {
        public async Task<IEnumerable<SolicitudCancelacion>> GetAll()
        {
            IEnumerable<SolicitudCancelacion> solicitudes;
            try
            {
                solicitudes = await context.SolicitudCancelacion
                    .Include(s => s.Recaudacion)
                    .ThenInclude(r => r != null ? r.Concepto : null) // Soluciona CS8602
                    .Include(s => s.UsuarioSolicita)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las solicitudes de cancelación: " + ex.InnerException.Message);
                throw new Exception("Error al obtener las solicitudes de cancelación: " + ex.Message);
            }
            return solicitudes;
        }        

        public async Task<SolicitudCancelacion?> GetById(int id)
        {
            SolicitudCancelacion? solicitud;
            try
            {
                solicitud = await context.SolicitudCancelacion
                    .Include(s => s.Recaudacion)
                    .ThenInclude(r => r != null ? r.Concepto : null) // Soluciona CS8602
                    .Include(s => s.UsuarioSolicita)
                    .FirstOrDefaultAsync(s => s.Id_solicitud == id) ?? throw new Exception("Solicitud no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la solicitud de cancelación: " + ex.InnerException.Message);
                throw new Exception("Error al obtener la solicitud de cancelación: " + ex.Message);
            }
            return solicitud;
        }

        public async Task<IEnumerable<SolicitudCancelacion>> Search(IQueryable<SolicitudCancelacion> query)
        {
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los lideres: " + ex.InnerException.Message);

                throw new Exception("Error al buscar los lideres: " + ex.Message);
            }
        }

        public async Task AddSolicitud(SolicitudCancelacion solicitud)
        {
            try
            {
                context.SolicitudCancelacion.Add(solicitud);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear la solicitud de cancelación: " + ex.InnerException.Message);
                throw new Exception("Error al crear la solicitud de cancelación: " + ex.Message);
            }
        }

        public async Task UpdateSolicitud(SolicitudCancelacion solicitud)
        {
            try
            {
                context.SolicitudCancelacion.Update(solicitud);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar la solicitud de cancelación: " + ex.InnerException.Message);
                throw new Exception("Error al actualizar la solicitud de cancelación: " + ex.Message);
            }
        }
    }
}
