using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Data.Access;
using Api.Entities;
using Api.Interfaces;   

namespace Api.Business
{
    public class BusinessRecaudacion : IRecaudacion
    {
        private readonly MySQLiteContext _context;
        private readonly DataRecaudacion _recaudacion;

        public BusinessRecaudacion(MySQLiteContext context)
        {
            _context = context;
            _recaudacion = new(_context);
        }

        public async Task<IEnumerable<Recaudacion>> GetAll()
        {
            return await _recaudacion.GetAll();
        }   

        public async Task<Recaudacion> GetById(int id)
        {
            return await _recaudacion.GetById(id);
        } 
        
        public async Task Create(int id_padron, int id_concepto, decimal monto,
            int id_cobrador, string folio_Recibo, string periodo_Inicio, string periodo_Fin,
            int id_tarifa, double? latitud, double? longitud)
        {
            Recaudacion cobro = new()
            {
                Id_padron = id_padron,
                Id_concepto = id_concepto,
                Monto = monto,
                Id_cobrador = id_cobrador,
                Fecha_cobro = DateTime.Now,
                Folio_Recibo = folio_Recibo,
                Latitud = latitud,
                Longitud = longitud
            };

            await _recaudacion.Create(cobro);
        }

        Task IRecaudacion.Update(int id, Recaudacion recaudacion)
        {
            throw new NotImplementedException();
        }

        Task IRecaudacion.Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
