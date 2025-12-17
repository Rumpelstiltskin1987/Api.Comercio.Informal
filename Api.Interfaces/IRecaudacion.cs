using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface IRecaudacion
    {
        public Task<IEnumerable<Recaudacion>> GetAll();
        public Task<Recaudacion> GetById(int id);

        public Task<IEnumerable<Recaudacion>> Search(int? idCobrador, int? idConcepto, DateTime? fechaInicio, DateTime? fechaFin);
        public Task Create(int id_padron, int id_gremio, int id_concepto, decimal monto,
            int id_cobrador, double? latitud, double? longitud);
        public Task Update(int id, Recaudacion recaudacion);
        public Task Delete(int id);
    }
}
