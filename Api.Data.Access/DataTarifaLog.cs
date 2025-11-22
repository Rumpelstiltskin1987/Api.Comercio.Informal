using Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Data.Access
{
    public class DataTarifaLog(MySQLiteContext context)
    {
        public async Task AddLog(TarifaLog tarifaLog)
        {
            try
            {
                context.TarifaLog.Add(tarifaLog);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log de la tarifa: " + ex.InnerException.Message);

                throw new Exception("Error al crear el log de la tarifa: " + ex.Message);
            }
        }

        public async Task<int> GetIdMovement(int id_tarifa)
        {
            int id_movimiento = 0;

            try
            {
                id_movimiento = await Task.Run(() =>
                    context.TarifaLog
                        .Where(x => x.Id_tarifa == id_tarifa)
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
