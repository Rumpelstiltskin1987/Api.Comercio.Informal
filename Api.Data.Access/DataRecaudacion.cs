using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataRecaudacion(MySQLiteContext context)
    {
        public async Task<IEnumerable<Recaudacion>> GetAll()
        {
            IEnumerable<Recaudacion> recaudacion;

            try
            {
                recaudacion = await context.Recaudacion
                    .Include(x => x.Padron)
                    .Include(x => x.Concepto)
                    .Include(x => x.Cobrador)
                    .Include(x => x.Padron.Gremio)
                    .ToListAsync();                
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la recaudación: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la recaudación: " + ex.Message);
            }

            return recaudacion;
        }

        public async Task<Recaudacion> GetById(int id)
        {
            Recaudacion cobro;

            try
            {
                cobro = await context.Recaudacion.FindAsync(id) ?? throw new Exception("Recaudación no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return cobro;
        }

        public async Task<Recaudacion> GetByFolio(string folio)
        {
            Recaudacion cobro;

            try
            {
                cobro = await context.Recaudacion
                    .Where(r => r.Folio_Recibo == folio)
                    .FirstOrDefaultAsync() ?? throw new Exception("Folio no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return cobro;
        }

        public async Task<IEnumerable<Recaudacion>> Search(IQueryable<Recaudacion> query)
        {
            try
            {
                return await query
                    .Include(x => x.Cobrador)
                    .Include(x => x.Padron)
                    .Include(x=> x.Concepto)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al buscar los registros: " + ex.InnerException.Message);

                throw new Exception("Error al buscar los registros: " + ex.Message);
            }
        }

        public async Task Create(Recaudacion recaudacion)
        {
            try
            {
                context.Recaudacion.Add(recaudacion);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al registrar la recaudación: " + ex.InnerException.Message);
                throw new Exception("Error al registrar la recaudación: " + ex.Message);
            }
        }

        public async Task Update(Recaudacion recaudacion)
        {
            try
            {
                context.Recaudacion.Update(recaudacion);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar la recaudación: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar la recaudación: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.Recaudacion.Remove(context.Recaudacion.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar la recaudación: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar la recaudación: " + ex.Message);
            }
        }        
    }
}
