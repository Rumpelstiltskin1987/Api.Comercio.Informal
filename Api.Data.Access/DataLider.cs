using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataLider(MySQLiteContext context)
    {
        public async Task<bool> Create(Lider lider)
        {
            bool result;

            try
            {
                context.Lider.Add(lider);
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
                context.Lider.Remove(context.Lider.Find(id)!);
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

        public async Task<IEnumerable<Lider>> GetAll()
        {
            IEnumerable<Lider> Lideres;

            try
            {
                Lideres = await context.Lider.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las categorias: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las categorias: " + ex.Message);
            }

            return Lideres;
        }

        public async Task<Lider> GetById(int id)
        {
            Lider lider;

            try
            {
                lider = await context.Lider.FindAsync(id) ?? throw new Exception("Categoria no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return lider;
        }

        public async Task<bool> Update(Lider lider)
        {
            bool result;

            try
            {
                context.Lider.Update(lider);
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
