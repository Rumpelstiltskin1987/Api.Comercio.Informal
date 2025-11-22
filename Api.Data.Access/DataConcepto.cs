using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataConcepto(MySQLiteContext context)
    {
        public async Task<IEnumerable<Concepto>> GetAll()
        {
            IEnumerable<Concepto> conceptos;

            try
            {
                conceptos = await context.Concepto.ToListAsync();

                if (!conceptos.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener los conceptos: " + ex.InnerException.Message);

                throw new Exception("Error al obtener los conceptos: " + ex.Message);
            }

            return conceptos;
        }

        public async Task<Concepto> GetById(int id)
        {
            Concepto concepto;

            try
            {
                concepto = await context.Concepto.FindAsync(id) ?? throw new Exception("Concepto no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el Concepto: " + ex.InnerException.Message);

                throw new Exception("Error al obtener el Concepto: " + ex.Message);
            }

            return concepto;
        }

        public async Task<IEnumerable<Concepto>> Search(IQueryable<Concepto> query)
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

        public async Task Create(Concepto Concepto)
        {
            try
            {
                context.Concepto.Add(Concepto);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el Concepto: " + ex.InnerException.Message);
                throw new Exception("Error al crear el Concepto: " + ex.Message);
            }
        }

        public async Task Update(Concepto Concepto)
        {
            try
            {
                context.Concepto.Update(Concepto);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el Concepto: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el Concepto: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.Concepto.Remove(context.Concepto.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el Concepto: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar el Concepto: " + ex.Message);
            }
        }
    }
}
