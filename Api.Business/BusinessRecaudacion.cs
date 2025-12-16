using Api.Data.Access;
using Api.Entities;
using Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Recaudacion> GetByFolio(string folio)
        {
            return await _recaudacion.GetByFolio(folio);
        }

        public async Task<IEnumerable<Recaudacion>> Search(int? idPadron, int? idConcepto, decimal? monto, int idCobrador,
            DateTime fechaCobro, string? estado)
        {
            var query = _context.Recaudacion.AsQueryable();
            if (idPadron > 0)
            {
                query = query.Where(t => t.Id_concepto == (idConcepto));
            }
            if (monto.HasValue)
            {
                query = query.Where(t => (decimal)t.Monto == monto.Value);
            }
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(t => t.Estado == estado);
            }
            return await _recaudacion.Search(query);
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
