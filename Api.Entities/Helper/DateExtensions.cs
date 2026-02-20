namespace Api.Entities.Helpers
{
    public static class DateExtensions
    {
        // Definimos la zona horaria de México como una constante estática para el rendimiento
        private static readonly TimeZoneInfo MexicoZone =
            TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

        /// <summary>
        /// Convierte una fecha UTC (de base de datos) a la hora local de México.
        /// </summary>
        public static DateTime ToLocal(this DateTime utcDate)
        {
            // Verificamos si la fecha ya es local para evitar dobles conversiones
            if (utcDate.Kind == DateTimeKind.Local) return utcDate;

            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, MexicoZone);
        }

        /// <summary>
        /// Formatea la fecha directamente a string local para reportes
        /// </summary>
        public static string ToLocalString(this DateTime utcDate, string format = "dd/MM/yyyy HH:mm")
        {
            return utcDate.ToLocal().ToString(format);
        }
    }
}
