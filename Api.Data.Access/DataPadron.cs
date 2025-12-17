using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataPadron(MySQLiteContext context)
    {
        public async Task<IEnumerable<Padron>> GetAll()
        {
            IEnumerable<Padron> padron;

            try
            {
                padron = await context.Padron
                    .Include(x => x.Gremio) 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el padron: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el padron: " + ex.Message);
            }

            return padron;
        }

        public async Task<Padron> GetById(int id)
        {
            Padron contribuyente;

            try
            {
                contribuyente = await context.Padron
                    .Include(x => x.Gremio)
                    .FirstOrDefaultAsync(x => x.Id_padron == id) ?? throw new Exception("Afiliado no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el afiliado: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el afiliado: " + ex.Message);
            }

            return contribuyente;
        }

        public async Task<IEnumerable<Padron>> Search(IQueryable<Padron> query)
        {
            try
            {
                return await query.Include(x => x.Gremio).ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los registros: " + ex.InnerException.Message);

                throw new Exception("Error al buscar los registros: " + ex.Message);
            }
        }

        public async Task Create(Padron contribuyente)
        {
            try
            {
                context.Padron.Add(contribuyente);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el afiliado: " + ex.InnerException.Message);

                throw new Exception("Error al crear el afiliado: " + ex.Message);
            }
        }

        public async Task Update(Padron contribuyente)
        {
            try
            {
                context.Padron.Update(contribuyente);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el afiliado: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el afiliado: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.Padron.Remove(context.Padron.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el afiliado: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar la afiliado: " + ex.Message);
            }
        }        
    }
}
