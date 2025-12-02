using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataLider(MySQLiteContext context)
    {
        public async Task<IEnumerable<Lider>> GetAll()
        {
            IEnumerable<Lider> lideres;

            try
            {
                lideres = await context.Lider.ToListAsync();

                
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los lideres: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los lideres: " + ex.Message);
            }

            return lideres;
        }

        public async Task<Lider> GetById(int id)
        {
            Lider lider;

            try
            {
                lider = await context.Lider.FindAsync(id) ?? throw new Exception("Lider no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el lider: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el lider: " + ex.Message);
            }

            return lider;
        }

        public async Task<IEnumerable<Lider>> Search(IQueryable<Lider> query)
        {
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los lideres: " + ex.InnerException.Message);

                throw new Exception("Error al buscar los lideres: " + ex.Message);
            }
        }

        public async Task Create(Lider lider)
        {
            try
            {
                context.Lider.Add(lider);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el lider: " + ex.InnerException.Message);
                throw new Exception("Error al crear el lider: " + ex.Message);
            }
        }

        public async Task Update(Lider lider)
        {
            try
            {
                context.Lider.Update(lider);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el lider: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el lider: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.Lider.Remove(context.Lider.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar los registros: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar los registros: " + ex.Message);
            }
        }
    }
}
