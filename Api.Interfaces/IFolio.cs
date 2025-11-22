using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.Interfaces
{
    public interface IFolio
    {
        public Task<IEnumerable<Folio>> GetAll();
        public Task<Folio> GetById(int id);
        public Task Create(int id_gremio, string descripcion, string prefijo);
        public Task Update(int id, int id_gremio, string descripcion, string prefijo,
            int siguiente_folio, int anio_vigente);
        public Task Delete(int id);
    }
}
