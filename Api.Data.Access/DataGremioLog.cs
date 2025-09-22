using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Data.Access
{
    public class DataGremioLog(MySQLiteContext context)
    {
        public async Task<bool> AddLog(GremioLog gremioLog)
        {
            bool result;
            try
            {
                context.GremioLog.Add(gremioLog);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log del gremio: " + ex.InnerException.Message);
                throw new Exception("Error al crear el log del gremio: " + ex.Message);
            }
            return result;
        }
        public async Task<int> GetIdMovement(int id_gremio)
        {
            int id_movimiento = 0;
            try
            {
                id_movimiento = await Task.Run(() =>
                    context.GremioLog
                        .Where(x => x.Id_gremio == id_gremio)
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
    }
}
