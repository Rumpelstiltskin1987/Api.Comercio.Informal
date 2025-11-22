using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Data.Access
{
    public class DataConceptoLog(MySQLiteContext context)
    {
        public async Task AddLog(ConceptoLog conceptoLog)
        {
            try
            {
                context.ConceptoLog.Add(conceptoLog);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el log del concepto: " + ex.InnerException.Message);

                throw new Exception("Error al crear el log del concepto: " + ex.Message);
            }
        }

        public async Task<int> GetIdMovement(int id_concepto)
        {
            int id_movimiento = 0;

            try
            {
                id_movimiento = await Task.Run(() =>
                    context.ConceptoLog
                        .Where(x => x.Id_concepto == id_concepto)
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
