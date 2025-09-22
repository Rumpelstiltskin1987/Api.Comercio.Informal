using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Data.Access
{
    public class DataCategoriaLog(MySQLiteContext context)
    {
        public async Task<bool> AddLog(CategoriaLog categoriaLog)
        {
            bool result;

            try
            {
                context.CategoriaLog.Add(categoriaLog);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log de la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al crear el log de la categoria: " + ex.Message);
            }

            return result;
        }

        public async Task<int> GetIdMovement(int id_categoria)
        {
            int id_movimiento = 0;

            try
            {
                id_movimiento = await Task.Run(() =>
                    context.CategoriaLog
                        .Where(x => x.Id_categoria == id_categoria)
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
