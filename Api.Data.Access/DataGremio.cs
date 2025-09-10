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
                    throw new Exception("Error al crear la categoria: " + ex.InnerException.Message);
                throw new Exception("Error al crear la categoria: " + ex.Message);
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
                    throw new Exception("Error al eliminar la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar la categoria: " + ex.Message);
            }

            return result;
        }

        public async Task<IEnumerable<Gremio>> GetAll()
        {
            IEnumerable<Gremio> gremios;

            try
            {
                gremios = await context.Gremio.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las categorias: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las categorias: " + ex.Message);
            }

            return gremios;
        }

        public async Task<Gremio> GetById(int id)
        {
            Gremio gremio;

            try
            {
                gremio = await context.Gremio.FindAsync(id) ?? throw new Exception("Categoria no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return gremio;
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
                    throw new Exception("Error al actualizar la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar la categoria: " + ex.Message);
            }

            return result;
        }
    }
}
