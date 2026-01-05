using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

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
