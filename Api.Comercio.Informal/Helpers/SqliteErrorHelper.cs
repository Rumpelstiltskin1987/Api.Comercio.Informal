using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Api.Comercio.Informal.Helpers

{
    public class SqliteErrorHelper
    {
        public static string ObtenerMensajeAmigable(Exception ex)
        {
            // 1. Buscamos si la causa raíz es una excepción de SQLite
            // A veces la excepción viene envuelta en una DbUpdateException
            var sqliteException = ex.InnerException as SqliteException;

            // Si no es directa, a veces está más adentro
            if (sqliteException == null && ex is DbUpdateException dbEx)
            {
                sqliteException = dbEx.InnerException as SqliteException;
            }

            // 2. Si encontramos que es error de SQLite
            if (sqliteException != null)
            {
                // Código 19 = Constraint Violation (Unique, Primary Key, Foreign Key)
                if (sqliteException.SqliteErrorCode == 19)
                {
                    return TraducirErrorUnique(sqliteException.Message);
                }
            }

            // 3. Si no es un error conocido de SQLite, devolvemos el mensaje original
            return ex.Message;
        }

        private static string TraducirErrorUnique(string mensajeTecnico)
        {
            // El mensaje técnico suele ser: "SQLite Error 19: 'UNIQUE constraint failed: Tabla.Campo'"

            // --- CASO 1: Solicitudes de Cancelación ---
            if (mensajeTecnico.Contains("SolicitudCancelacion.Id_recaudacion"))
            {
                return "Ya existe una solicitud de cancelación pendiente o procesada para este recibo de cobro.";
            }

            // --- CASO 2: Conceptos ---
            if (mensajeTecnico.Contains("Concepto.Descripcion"))
            {
                return "Ya existe un concepto registrado con esa misma descripción.";
            }

            // --- CASO 3: Padron / Matricula ---
            if (mensajeTecnico.Contains("Padron.Matricula"))
            {
                return "Esta matrícula ya está asignada a otro contribuyente.";
            }

            // --- CASO 4: Usuarios ---
            if (mensajeTecnico.Contains("AspNetUsers.UserName"))
            {
                return "Este nombre de usuario ya está ocupado.";
            }

            // --- CASO 5: Tarifas ---
            if (mensajeTecnico.Contains("Tarifa.Id_concepto") && mensajeTecnico.Contains("Tarifa.Id_gremio"))
            {
                return "Ya existe una tarifa registrada para este gremio y concepto.";
            }

            // Default: Si es un error unique pero no lo tenemos mapeado
            return "El registro que intentas guardar ya existe en la base de datos (Dato duplicado).";
        }

    }
}
