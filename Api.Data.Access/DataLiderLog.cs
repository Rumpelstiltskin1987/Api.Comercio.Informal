using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Data.Access
{
    public class DataLiderLog(MySQLiteContext context)
    {
        public async Task<bool> AddLog(LiderLog liderLog)
        {
            bool result;
            try
            {
                context.LiderLog.Add(liderLog);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log del lider: " + ex.InnerException.Message);
                throw new Exception("Error al crear el log del lider: " + ex.Message);
            }
            return result;
        }
        public async Task<int> GetIdMovement(int id_lider)
        {
            int id_movimiento = 0;
            try
            {
                id_movimiento = await Task.Run(() =>
                    context.LiderLog
                        .Where(x => x.Id_lider == id_lider)
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
