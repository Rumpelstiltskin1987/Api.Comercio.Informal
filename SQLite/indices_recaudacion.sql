/* 1. Índice para el Corte de Caja (El más importante) */
/* Optimiza las búsquedas combinadas de Cobrador y Rango de Fechas. 
   Se pone Id_cobrador primero porque suele ser un filtro de igualdad (=). */
CREATE INDEX IX_Recaudacion_Cobrador_Fecha 
ON Recaudacion (Id_cobrador, Fecha_cobro);

CREATE INDEX IX_Recaudacion_Fecha_Filtro 
ON Recaudacion (Fecha_cobro);

/* 2. Índice para el Padrón / Contribuyente */
/* Optimiza las consultas que listan los cobros realizados a un contribuyente específico. */
CREATE INDEX IX_Recaudacion_Padron 
ON Recaudacion (Id_padron);

/* 3. Índice para Conceptos */
/* Útil si en el futuro decides filtrar reportes por tipo de concepto (ej. Piso, Limpieza). */
CREATE INDEX IX_Recaudacion_Concepto 
ON Recaudacion (Id_concepto);

/* 4. Índice de Estado */
/* Útil para filtrar rápidamente registros activos ('A') de los cancelados ('C') 
   en reportes financieros. */
CREATE INDEX IX_Recaudacion_Estado 
ON Recaudacion (Estado);

/* Nota: No creamos índice para Folio_Recibo porque al definirlo como UNIQUE 
   en el CREATE TABLE, SQLite ya crea automáticamente un índice para él. */