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
                padron = await context.Padron.ToListAsync();
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
            Padron vendedor;

            try
            {
                vendedor = await context.Padron.FindAsync(id) ?? throw new Exception("Afiliado no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el afiliado: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el afiliado: " + ex.Message);
            }

            return vendedor;
        }

        public async Task<IEnumerable<Padron>> Search(IQueryable<Padron> query)
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

        public async Task Create(Padron padron)
        {
            try
            {
                context.Padron.Add(padron);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el afiliado: " + ex.InnerException.Message);

                throw new Exception("Error al crear el afiliado: " + ex.Message);
            }
        }

        public async Task Update(Padron padron)
        {
            try
            {
                context.Padron.Update(padron);
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
