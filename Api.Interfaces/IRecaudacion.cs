using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Entities.DTO;

namespace Api.Interfaces
{
    public interface IRecaudacion
    {
        public Task<IEnumerable<Recaudacion>> GetAll();
        public Task<Recaudacion> GetById(int id);

        public Task<IEnumerable<Recaudacion>> Search(int? idCobrador, int? idConcepto, DateTime? fechaInicio, 
            DateTime? fechaFin, string? estado);
        public Task Create(DtoRecaudacionCrear cobro);
        public Task Update(int id, Recaudacion recaudacion);
        public Task Delete(int id);
    }
}
