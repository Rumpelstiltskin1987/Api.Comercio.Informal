using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Data.Access
{
    public class DataCuotaLog(MySQLiteContext context)
    {
        public async Task<bool> AddLog(CuotaLog cuotaLog)
        {
            bool result;
            try
            {
                context.CuotaLog.Add(cuotaLog);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log de la cuota: " + ex.InnerException.Message);
                throw new Exception("Error al crear el log de la cuota: " + ex.Message);
            }
            return result;
        }

        public async Task<int> GetIdMovement(int id_cuota)
        {
            int id_movimiento = 0;
            try
            {
                id_movimiento = await Task.Run(() =>
                    context.CuotaLog
                        .Where(x => x.Id_cuota == id_cuota)
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
