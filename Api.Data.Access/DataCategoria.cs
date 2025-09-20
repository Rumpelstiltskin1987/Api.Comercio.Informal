using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataCategoria(MySQLiteContext context)
    {
        public async Task<IEnumerable<Categoria>> GetAll()
        {
            IEnumerable<Categoria> categorias;

            try
            {
                categorias = await context.Categoria.ToListAsync();

                if (!categorias.Any())  
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las categorias: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las categorias: " + ex.Message);
            }

            return categorias;
        }

        public async Task<Categoria> GetById(int id)
        {
            Categoria categoria;

            try
            {
                categoria = await context.Categoria.FindAsync(id) ?? throw new Exception("Categoria no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return categoria;
        }

        public async Task<bool> Create(Categoria categoria)
        {
            bool result;

            try
            {
                context.Categoria.Add(categoria);
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

        public async Task<bool> Update(Categoria categoria)
        {
            bool result;

            try
            {
                context.Categoria.Update(categoria);
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
                context.Categoria.Remove(context.Categoria.Find(id)!);
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
