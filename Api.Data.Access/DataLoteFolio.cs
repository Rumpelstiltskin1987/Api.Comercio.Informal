using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Access
{
    public class DataLoteFolio(MySQLiteContext context)
    {
        public async Task<IEnumerable<LoteFolio>> GetAll()
        {
            IEnumerable<LoteFolio> lotes;

            try
            {
                lotes = await context.LoteFolio.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Data Access: " + ex.InnerException.Message);

                throw new Exception("Data Access: " + ex.Message);
            }

            return lotes;
        }

        public async Task<LoteFolio> GetById(int id)
        {
            LoteFolio lote;

            try
            {
                lote = await context.LoteFolio.FindAsync(id) ?? throw new Exception("Lote no encontrado");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Data Access: " + ex.InnerException.Message);

                throw new Exception("Data Access: " + ex.Message);
            }

            return lote;
        }

        public async Task<IEnumerable<LoteFolio>> Search(IQueryable<LoteFolio> query)
        {
            try
            {
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Data Access: " + ex.InnerException.Message);

                throw new Exception("Data Access: " + ex.Message);
            }
        }

        public async Task Create(LoteFolio lote)
        {
            try
            {
                context.LoteFolio.Add(lote);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                context.Entry(lote).State = EntityState.Detached;
                if (ex.InnerException != null)
                    throw new Exception("Data Access: " + ex.InnerException.Message);
                throw new Exception("Data Access: " + ex.Message);
            }
        }

        public async Task Update(LoteFolio lote)
        {
            try
            {
                context.LoteFolio.Update(lote);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                context.Entry(lote).State = EntityState.Detached;
                if (ex.InnerException != null)
                    throw new Exception("Error al actualizar el LoteFolio: " + ex.InnerException.Message);

                throw new Exception("Error al actualizar el LoteFolio: " + ex.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                context.LoteFolio.Remove(context.LoteFolio.Find(id)!);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    throw new Exception("Error al eliminar el LoteFolio: " + ex.InnerException.Message);

                throw new Exception("Error al eliminar el LoteFolio: " + ex.Message);
            }
        }        
    }
}
