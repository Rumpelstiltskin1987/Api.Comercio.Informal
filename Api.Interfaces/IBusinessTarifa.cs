using Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Business
{
    public interface IBusinessTarifa
    {
        public Task<IEnumerable<Tarifa>> GetAll();
        public Task<Tarifa> GetById(int id);
        public Task Create(int id_concepto, int id_gremio, decimal? monto, string usuario);
        public Task Update(int id, int id_concepto, int id_gremio, decimal? monto, string estado,
            string usuario);
        public Task Delete(int id);
    }
}