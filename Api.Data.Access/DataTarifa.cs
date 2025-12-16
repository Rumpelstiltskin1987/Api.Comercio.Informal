using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataTarifa(MySQLiteContext context)
    {
        public async Task<IEnumerable<Tarifa>> GetAll()
        {
            IEnumerable<Tarifa> tarifas;

            try
            {
                tarifas = await context.Tarifa.Include(x => x.Concepto).
                    Include(x => x.Gremio).ToListAsync();


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las tarifas: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las tarifas: " + ex.Message);
            }

            return tarifas;
        }

        public async Task<Tarifa> GetById(int id)
        {
            Tarifa tarifa;

            try
            {
                tarifa = await context.Tarifa.FindAsync(id) ?? throw new Exception("Tarifa no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la tarifa: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la tarifa: " + ex.Message);
            }

            return tarifa;
        }

        public async Task<IEnumerable<Tarifa>> Search(IQueryable<Tarifa> query)
        {
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los registros: " + ex.InnerException.Message);

                throw new Exception("Error al buscar los registros: " + ex.Message);
            }
        }

        public async Task Create(Tarifa tarifa)
        {
            try
            {
                context.Tarifa.Add(tarifa);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear la tarifa: " + ex.InnerException.Message);

                throw new Exception("Error al crear la tarifa: " + ex.Message);
            }
        }

        public async Task Update(Tarifa tarifa)
        {
            try
            {
                context.Tarifa.Update(tarifa);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar la tarifa: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar la tarifa: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.Tarifa.Remove(context.Tarifa.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar la tarifa: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar la tarifa: " + ex.Message);
            }
        }
    }
}
