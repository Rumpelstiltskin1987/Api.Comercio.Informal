using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataMatriculaContador(MySQLiteContext context)
    {
        public async Task<IEnumerable<MatriculaContador>> GetAll()
        {
            IEnumerable<MatriculaContador> matriculas;

            try
            {
                matriculas = await context.MatriculaContador.ToListAsync();

                if (!matriculas.Any())
                    throw new Exception("No existen registros en la base de datos.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener las matrículas: " + ex.InnerException.Message);

                throw new Exception("Error al obtener las matrículas: " + ex.Message);
            }

            return matriculas;
        }

        public async Task<MatriculaContador> GetById(int id)
        {
            MatriculaContador matriculaContador;

            try
            {
                matriculaContador = await context.MatriculaContador.FindAsync(id) ?? throw new Exception("Cobrador no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al obtener la matrícula: " + ex.InnerException.Message);

                throw new Exception("Error al obtener la matrícula: " + ex.Message);
            }

            return matriculaContador;
        }

        public async Task<IEnumerable<MatriculaContador>> Search(IQueryable<MatriculaContador> query)
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

        public async Task Create(MatriculaContador matriculaContador)
        {
            try
            {
                context.MatriculaContador.Add(matriculaContador);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al crear la matrícula: " + ex.InnerException.Message);

                throw new Exception("Error al crear la matrícula: " + ex.Message);
            }
        }

        public async Task Update(MatriculaContador matriculaContador)
        {
            try
            {
                context.MatriculaContador.Update(matriculaContador);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar la matrícula: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar la matrícula: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.MatriculaContador.Remove(context.MatriculaContador.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar la matrícula: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar la matrícula: " + ex.Message);
            }
        }
    }
}
