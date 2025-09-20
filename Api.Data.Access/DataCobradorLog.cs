using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Data.Access
{
    public class DataCobradorLog(MySQLiteContext context)
    {
        public async Task<bool> AddLog(CobradorLog cobradorLog)
        {
            bool result;
            try
            {
                context.CobradorLog.Add(cobradorLog);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log del cobrador: " + ex.InnerException.Message);
                throw new Exception("Error al crear el log del cobrador: " + ex.Message);
            }
            return result;
        }
        public async Task<int> GetIdMovement(int id_cobrador)
        {
            int id_movimiento = 0;
            try
            {
                id_movimiento = await Task.Run(() =>
                    context.CobradorLog
                        .Where(x => x.Id_cobrador == id_cobrador)
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
