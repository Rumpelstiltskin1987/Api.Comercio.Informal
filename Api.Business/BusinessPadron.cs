using Api.Data.Access;
using Api.Entities;
using Api.Entities.DTO;
using Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Api.Business
{
    public class BusinessPadron : IPadron
    {
        private readonly MySQLiteContext _context;
        private readonly DataPadron _dataPadron;
        private readonly DataPadronLog _dataPadronLog;
        private readonly DataMatriculaContador _dataMatriculaContador;
        private readonly DataGremio _gremio;

        public BusinessPadron(MySQLiteContext context)
        {
            _context = context;
            _dataPadron = new(_context);
            _dataPadronLog = new(_context);
            _dataMatriculaContador = new(_context);
            _gremio = new(_context);
        }

        public async Task<IEnumerable<Padron>> GetAll()
        {
            return await _dataPadron.GetAll();
        }

        public async Task<Padron> GetById(int id)
        {
            return await _dataPadron.GetById(id);
        }

        public async Task<IEnumerable<Padron>> Search(string? nombre, string? aPaterno, string? aMaterno,
            string? curp, string? matricula, int idGremio, string? tipo, string? estado)
        {
            var query = _context.Padron.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(c => c.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(aPaterno))
            {
                query = query.Where(c => c.A_paterno.Contains(aPaterno));
            }

            if (!string.IsNullOrEmpty(aMaterno))
            {
                query = query.Where(c => c.A_materno.Contains(aMaterno));
            }

            if (!string.IsNullOrEmpty(curp))
            {
                query = query.Where(c => c.Curp.Contains(curp));
            }

            if (!string.IsNullOrEmpty(matricula))
            {
                query = query.Where(c => c.Matricula.Contains(matricula));
            }

            if (idGremio > 0)
            {
                query = query.Where(c => c.Id_gremio == idGremio);
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                query = query.Where(c => c.Tipo_vendedor == tipo);
            }

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(c => c.Estado == estado);
            }            

            return await _dataPadron.Search(query);
        }

        public async Task Create(string nombre, string a_paterno, string a_materno, string curp,
            string direccion, string telefono, string? email, int id_gremio, string tipo, string usuario)
        {
            MatriculaContador matriculaContador = await _dataMatriculaContador.GetByType(tipo);
            string matricula;            

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (matriculaContador == null)
                {
                    matriculaContador = new()
                    {
                        Tipo_vendedor = tipo,
                        Anio = DateTime.Now.Year,
                        Siguiente_numero = 1
                    };
                    matricula = tipo + (DateTime.Now.Year % 100) + matriculaContador.Siguiente_numero.ToString("D5");
                    await _dataMatriculaContador.Create(matriculaContador);


                }
                else
                {
                    matriculaContador.Siguiente_numero += 1;
                    matricula = tipo + DateTime.Now.Year % 100 + matriculaContador.Siguiente_numero.ToString("D5");
                    await _dataMatriculaContador.Update(matriculaContador);
                }

                Padron contribuyente = new()
                {                    
                    Nombre = nombre,
                    A_paterno = a_paterno,
                    A_materno = a_materno,
                    Curp = curp,
                    Direccion = direccion,
                    Telefono = telefono,
                    Email = email,
                    Matricula = matricula,
                    Id_gremio = id_gremio,
                    Tipo_vendedor = tipo,
                    Estado = "A",
                    Usuario_alta = usuario,
                    Fecha_alta = DateTime.Now
                };
                await _dataPadron.Create(contribuyente);

                var gremio = await _gremio.GetById(id_gremio);

                PadronLog padronLog = new()
                {
                    Id_movimiento = 1,
                    Id_padron = contribuyente.Id_padron,                    
                    Nombre = contribuyente.Nombre,
                    A_paterno = contribuyente.A_paterno,
                    A_materno = contribuyente.A_materno,
                    Curp = contribuyente.Curp,
                    Direccion = contribuyente.Direccion,
                    Telefono = contribuyente.Telefono,
                    Email = contribuyente.Email,
                    Matricula = contribuyente.Matricula,
                    Matricula_anterior = contribuyente.Matricula_anterior,
                    Gremio = gremio.Descripcion,                    
                    Tipo_vendedor = contribuyente.Tipo_vendedor,
                    Estado = contribuyente.Estado,
                    Tipo_movimiento = "A",
                    Usuario_modificacion = contribuyente.Usuario_alta,
                    Fecha_modificacion = contribuyente.Fecha_alta,
                };

                await _dataPadronLog.AddLog(padronLog);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Update(int id, string nombre, string a_paterno, string a_materno, string curp, 
            string direccion, string telefono, string? email, string matricula, string? matricula_anterior, 
            int id_gremio, string status, string usuario)
        {
            Padron padron = await _dataPadron.GetById(id);

            padron.Matricula = matricula;
            padron.Nombre = nombre;
            padron.A_paterno = a_paterno;
            padron.A_materno = a_materno;
            padron.Curp = curp;
            padron.Direccion = direccion;
            padron.Telefono = telefono;
            padron.Email = email;
            padron.Id_gremio = id_gremio;
            padron.Estado = status;
            padron.Usuario_modificacion = usuario;
            padron.Fecha_modificacion = DateTime.Now;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _dataPadron.Update(padron);
                int idMovimiento = await _dataPadronLog.GetIdMovement(id) + 1;
                var gremio = await _gremio.GetById(id_gremio);

                PadronLog padronLog = new()
                {
                    Id_movimiento = idMovimiento,
                    Id_padron = padron.Id_padron,
                    Matricula = padron.Matricula,
                    Nombre = padron.Nombre,
                    A_paterno = padron.A_paterno,
                    A_materno = padron.A_materno,
                    Curp = padron.Curp,
                    Direccion = padron.Direccion,
                    Telefono = padron.Telefono,
                    Email = padron.Email,
                    Gremio = gremio.Descripcion,
                    Estado = padron.Estado,
                    Tipo_movimiento = "M",
                    Usuario_modificacion = padron.Usuario_modificacion,
                    Fecha_modificacion = padron.Fecha_modificacion
                };

                await _dataPadronLog.AddLog(padronLog);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            _ = await _dataPadron.GetById(id);
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _dataPadron.Delete(id);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<DtoHistorial>> GetHistorial(int id)
        {
            try
            {
                // 1. Obtenemos la lista cruda de la base de datos
                var logs = await _dataPadronLog.GetLogsByPadronId(id);

                // 2. Transformamos (Mapeamos) cada UsuarioLog a DtoHistorial
                var historial = logs.Select(log => new DtoHistorial
                {
                    Fecha = log.Fecha_modificacion,
                    Usuario = log.Usuario_modificacion,
                    Movimiento = log.Tipo_movimiento.ToUpper() switch
                    {
                        "A" => "Alta",
                        "M" => "Modificación",
                        _ => log.Tipo_movimiento
                    },
                    Detalles = $"Matrícula: {log.Matricula} " +
                               $"| Nombre: {log.Nombre} {log.A_paterno} {log.A_materno}" +
                               $"| CURP: {log.Curp} " +
                               $"| Dirección: {log.Direccion} " +
                               $"| Gremio: {log.Gremio}" +
                               $"| Teléfono: {log.Telefono} " +
                               $"| Estado: {(log.Estado == "A" ? "Activo" : (log.Estado == "I" ? "Inactivo" : log.Estado))} " +
                               $"| Email: {log.Email}"
                }).ToList();

                return historial;
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}
