DROP TABLE LoteFolio;

CREATE TABLE LoteFolio (
    Id_lote INTEGER PRIMARY KEY AUTOINCREMENT,
    Id_usuario INTEGER NOT NULL,
    Id_gremio INTEGER NOT NULL,
    Rango_inicial INTEGER NOT NULL, -- Ej: 1
    Rango_final INTEGER NOT NULL,   -- Ej: 50
    Ultimo_usado INTEGER NOT NULL,  -- Para saber por cuál va
    Anio INTEGER NOT NULL,
    Estado TEXT DEFAULT 'ACTIVO',   -- ACTIVO, AGOTADO
    Fecha_asignacion DATETIME DEFAULT CURRENT_TIMESTAMP,
	Fecha_modificacion DATETIME DEFAULT CURRENT_TIMESTAMP
);


select * from LoteFolio