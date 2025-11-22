using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataCobrador(MySQLiteContext context)
    {
        public async Task<IEnumerable<Cobrador>> GetAll()
        {
            IEnumerable<Cobrador> cobradores;

            try
            {
                cobradores = await context.Cobrador.ToListAsync();

                if (!cobradores.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los cobradores: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los cobradores: " + ex.Message);
            }

            return cobradores;
        }

        public async Task<Cobrador> GetById(int id)
        {
            Cobrador? cobrador;

            try
            {
                cobrador = await context.Cobrador.FindAsync(id) ?? throw new Exception("Cobrador no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el cobrador: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el cobrador: " + ex.Message);
            }

            return cobrador;
        }

        public async Task<IEnumerable<Cobrador>> Search(IQueryable<Cobrador> query)
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

        public async Task Create(Cobrador cobrador)
        {
            try
            {
                context.Cobrador.Add(cobrador);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el cobrador: " + ex.InnerException.Message);

                throw new Exception("Error al crear el cobrador: " + ex.Message);
            }
        }

        public async Task Update(Cobrador cobrador)
        {
            try
            {
                context.Cobrador.Update(cobrador);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el cobrador: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el cobrador: " + ex.Message);
            }
        }        

        public async Task Delete(int id)
        {
            try
            {
                context.Cobrador.Remove(context.Cobrador.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el cobrador: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar el cobrador: " + ex.Message);
            }
        }        
    }
}
