-- 1. Renombramos la tabla actual para respaldarla
ALTER TABLE SolicitudCancelacion RENAME TO SolicitudCancelacion_Old;

-- 2. Creamos la tabla corregida (SIN el UNIQUE en Id_recaudacion)
CREATE TABLE SolicitudCancelacion(
    Id_solicitud INTEGER PRIMARY KEY AUTOINCREMENT,
    Id_recaudacion INTEGER NOT NULL, -- <--- Eliminamos la palabra UNIQUE de aquí
    Id_usuario_solicita INTEGER NOT NULL,
    Fecha_solicitud TEXT NOT NULL,
    Motivo_solicitud TEXT NOT NULL,
    Estado_Solicitud TEXT NOT NULL CHECK (Estado_solicitud IN ('P','A','R')),
    Id_usuario_responde INTEGER,
    Fecha_respuesta TEXT,
    Motivo_respuesta TEXT ,    
    FOREIGN KEY (Id_usuario_solicita) REFERENCES AspNetUsers(Id)
);

-- 3. Pasamos los datos de la tabla vieja a la nueva
INSERT INTO SolicitudCancelacion 
SELECT * FROM SolicitudCancelacion_Old;

-- 4. Eliminamos la tabla vieja
DROP TABLE SolicitudCancelacion_Old;

-- 5. EL BLINDAJE: Creamos el índice único parcial
CREATE UNIQUE INDEX UX_Solicitud_Pendiente_Unica 
ON SolicitudCancelacion(Id_recaudacion) 
WHERE Estado_Solicitud = 'P';