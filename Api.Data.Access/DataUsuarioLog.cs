using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Entities.DTO;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataUsuarioLog(MySQLiteContext context)
    {
        public async Task AddLog(UsuarioLog log)
        {
            try
            {
                context.UsuarioLog.Add(log);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log: " + ex.InnerException.Message);
                throw new Exception("Error al crear el log: " + ex.Message);
            }
        }
        public async Task<int> GetIdMovement(int id)
        {
            int id_movimiento = 0;
            try
            {
                id_movimiento = await Task.Run(() =>
                    context.UsuarioLog
                        .Where(x => x.Id == id)
                        .Max(x => (int?)x.Id_movimiento) ?? 0
                );
                return id_movimiento;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el id de movimiento: " + ex.InnerException.Message);
                throw new Exception("Error al obtener el id de movimiento: " + ex.Message);
            }            
        }

        public async Task<IEnumerable<UsuarioLog>> GetLogsByUserId(int id)
        {
            IEnumerable<UsuarioLog> historial;
            try
            {
                historial = await context.UsuarioLog.Where(x => x.Id == id).ToListAsync();
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
