using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataConcepto(MySQLiteContext context)
    {
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
                context.Concepto.Remove(context.Concepto.Find(id)!);
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

        public async Task<IEnumerable<Concepto>> GetAll()
        {
            IEnumerable<Concepto> concepto;

            try
            {
                concepto = await context.Concepto.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las categorias: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las categorias: " + ex.Message);
            }

            return concepto;
        }

        public async Task<Concepto> GetById(int id)
        {
            Concepto concepto;

            try
            {
                concepto = await context.Concepto.FindAsync(id) ?? throw new Exception("Categoria no encontrada");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la categoria: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return concepto;
        }

        public async Task<bool> Update(Concepto concepto)
        {
            bool result;

            try
            {
                context.Concepto.Update(concepto);
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
