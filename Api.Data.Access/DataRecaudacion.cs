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
            IEnumerable<Recaudacion> Recaudaciones;

            try
            {
                Recaudaciones = await context.Recaudacion.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las categorias: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las categorias: " + ex.Message);
            }

            return Recaudaciones;
        }

        public async Task<Recaudacion> GetById(int id)
        {
            Recaudacion Recaudacion;

            try
            {
                Recaudacion = await context.Recaudacion.FindAsync(id) ?? throw new Exception("Categoria no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return Recaudacion;
        }

        public async Task<bool> Create(Recaudacion recaudacion)
        {
            bool result;

            try
            {
                context.Recaudacion.Add(recaudacion);
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

        public async Task<bool> Update(Recaudacion recaudacion)
        {
            bool result;

            try
            {
                context.Recaudacion.Update(recaudacion);
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

        public async Task<bool> Delete(int id)
        {
            bool result;

            try
            {
                context.Recaudacion.Remove(context.Recaudacion.Find(id)!);
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
    }
}
