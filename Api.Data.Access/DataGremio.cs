using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataGremio(MySQLiteContext context)
    {

        public async Task<IEnumerable<Gremio>> GetAll()
        {
            IEnumerable<Gremio> gremios;

            try
            {
                gremios = await context.Gremio.Include(x => x.Lider).ToListAsync();

                if (!gremios.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los gremios: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los gremios: " + ex.Message);
            }

            return gremios;
        }

        public async Task<Gremio> GetById(int id)
        {
            Gremio gremio;

            try
            {
                gremio = await context.Gremio.FindAsync(id) ?? throw new Exception("Gremio no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el gremio: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el gremio: " + ex.Message);
            }

            return gremio;
        }

        public async Task<IEnumerable<Gremio>> Search(IQueryable<Gremio> query)
        {
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los registros: " + ex.InnerException.Message);

                throw new Exception("Error al buscar los los registros: " + ex.Message);
            }
        }

        public async Task<bool> Create(Gremio gremio)
        {
            bool result;

            try
            {
                context.Gremio.Add(gremio);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el gremio: " + ex.InnerException.Message);
                throw new Exception("Error al crear el gremio: " + ex.Message);
            }

            return result;
        }

        public async Task<bool> Update(Gremio gremio)
        {
            bool result;

            try
            {
                context.Gremio.Update(gremio);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el gremio: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el gremio: " + ex.Message);
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result;

            try
            {
                context.Gremio.Remove(context.Gremio.Find(id)!);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el gremio: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar el gremio: " + ex.Message);
            }

            return result;
        }        
    }
}
