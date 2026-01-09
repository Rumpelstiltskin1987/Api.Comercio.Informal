using Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access
{
    public class DataPadronLog(MySQLiteContext context)
    {
        public async Task AddLog(PadronLog log)
        {
            try
            {
                context.PadronLog.Add(log);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log del padron: " + ex.InnerException.Message);
                throw new Exception("Error al crear el log del padron: " + ex.Message);
            }
        }
        public async Task<int> GetIdMovement(int id_padron)
        {
            int id_movimiento = 0;
            try
            {
                id_movimiento = await Task.Run(() =>
                    context.PadronLog
                        .Where(x => x.Id_padron == id_padron)
                        .Max(x => (int?)x.Id_movimiento) ?? 0
                );
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el id de movimiento: " + ex.InnerException.Message);
                throw new Exception("Error al obtener el id de movimiento: " + ex.Message);
            }
            return id_movimiento;
        }

        public async Task<IEnumerable<PadronLog>> GetLogsByPadronId(int id)
        {
            IEnumerable<PadronLog> historial;
            try
            {
                historial = await context.PadronLog.Where(x => x.Id_padron == id).ToListAsync();
                return historial;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el historial: " + ex.InnerException.Message);
                throw new Exception("Error al obtener el historial: " + ex.Message);
            }
        }
    }
}
