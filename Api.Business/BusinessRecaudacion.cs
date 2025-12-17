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
        private readonly DataFolio _dataFolio;

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

        public async Task Create(int id_padron, int id_gremio, int id_concepto, decimal monto,
    int id_cobrador, double? latitud, double? longitud)
        {

            var queryFolio = _context.Folio.AsQueryable().Where(f => f.Id_gremio == id_gremio);
            var listaFolios = await _dataFolio.Search(queryFolio);
            var folioEncontrado = listaFolios.FirstOrDefault();

            if (folioEncontrado == null)
            {
                throw new Exception("No se encontró configuración de folios para el gremio especificado.");
            }

            string folioRecibo = $"{folioEncontrado.Prefijo}{folioEncontrado.Anio_vigente % 100} - {folioEncontrado.Siguiente_folio:D6}";

            Recaudacion cobro = new()
            {
                Id_padron = id_padron,
                Id_concepto = id_concepto,
                Monto = monto,
                Id_cobrador = id_cobrador,
                Fecha_cobro = DateTime.Now,
                Folio_Recibo = folioRecibo,
                Latitud = latitud,
                Longitud = longitud
            };

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                folioEncontrado.Siguiente_folio += 1;

                await _recaudacion.Create(cobro);               
                await _dataFolio.Update(folioEncontrado);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
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
