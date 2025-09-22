using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataCuota(MySQLiteContext context)
    {
        public async Task<IEnumerable<Cuota>> GetAll()
        {
            IEnumerable<Cuota> cuotas;

            try
            {
                cuotas = await context.Cuota.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las cuotas: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las cuotas: " + ex.Message);
            }

            return cuotas;
        }

        public async Task<Cuota> GetById(int id)
        {
            Cuota cuota;

            try
            {
                cuota = await context.Cuota.FindAsync(id) ?? throw new Exception("Cuota no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la cuota: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la cuota: " + ex.Message);
            }

            return cuota;
        }
        public async Task<bool> Create(Cuota cuota)
        {
            bool result;

            try
            {
                context.Cuota.Add(cuota);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear la cuota: " + ex.InnerException.Message);
                throw new Exception("Error al crear la cuota: " + ex.Message);
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result;

            try
            {
                context.Cuota.Remove(context.Cuota.Find(id)!);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar la cuota: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar la cuota: " + ex.Message);
            }

            return result;
        }          

        public async Task<bool> Update(Cuota cuota)
        {
            bool result;

            try
            {
                context.Cuota.Update(cuota);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar la cuota: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar la cuota: " + ex.Message);
            }

            return result;
        }
    }
}
