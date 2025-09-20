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
            Concepto? concepto;
            try
            {
                concepto = await context.Concepto.FindAsync(id) ?? throw new Exception("Concepto no encontrado"); ;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener el concepto: " + ex.InnerException.Message);
                throw new Exception("Error al obtener el concepto: " + ex.Message);
            }
            return concepto;
        }

        public async Task<bool> Create(Concepto concepto)
        {
            bool result;
            try
            {
                context.Concepto.Add(concepto);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear el concepto: " + ex.InnerException.Message);
                throw new Exception("Error al crear el concepto: " + ex.Message);
            }
            return result;
        }

        public async Task<bool> Update(Concepto concepto)
        {
            bool result;
            try
            {
                var existingConcepto = await context.Concepto.FindAsync(concepto.Id_concepto) ?? throw new Exception("Concepto no encontrado");
                existingConcepto.Descripcion = concepto.Descripcion;
                existingConcepto.Estado = concepto.Estado;
                existingConcepto.Usuario_modificacion = concepto.Usuario_modificacion;
                existingConcepto.Fecha_modificacion = DateTime.Now;
                context.Concepto.Update(existingConcepto);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el concepto: " + ex.InnerException.Message);
                throw new Exception("Error al actualizar el concepto: " + ex.Message);
            }
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result;
            try
            {
                var concepto = await context.Concepto.FindAsync(id) ?? throw new Exception("Concepto no encontrado");
                context.Concepto.Remove(concepto);
                await context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el concepto: " + ex.InnerException.Message);
                throw new Exception("Error al eliminar el concepto: " + ex.Message);
            }
            return result;
        }
    }
}
