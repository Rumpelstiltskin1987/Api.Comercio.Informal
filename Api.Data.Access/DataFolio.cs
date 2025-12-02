using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataFolio(MySQLiteContext context)
    {
        public async Task<IEnumerable<Folio>> GetAll()
        {
            IEnumerable<Folio> folios;

            try
            {
                folios = await context.Folio.Include(x => x.Gremio).ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los folios: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los folios: " + ex.Message);
            }

            return folios;
        }

        public async Task<Folio> GetById(int id)
        {
            Folio folio;

            try
            {
                folio = await context.Folio.FindAsync(id) ?? throw new Exception("Folio no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el folio: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el folio: " + ex.Message);
            }

            return folio;
        }

        public async Task<IEnumerable<Folio>> Search(IQueryable<Folio> query)
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

        public async Task Create(Folio folio)
        {
            try
            {
                context.Folio.Add(folio);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el folio: " + ex.InnerException.Message);
                throw new Exception("Error al crear el folio: " + ex.Message);
            }
        }

        public async Task Update(Folio folio)
        {
            try
            {
                context.Folio.Update(folio);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el folio: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el folio: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.Folio.Remove(context.Folio.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el folio: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar el folio: " + ex.Message);
            }
        }        
    }
}
