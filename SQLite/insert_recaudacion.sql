INSERT INTO Recaudacion (
    Id_padron, 
    Id_concepto, 
    Monto, 
    Id_cobrador, 
    Fecha_cobro, 
    Folio_Recibo, 
    Estado, 
    Latitud, 
    Longitud
) VALUES (
    1,                        -- Id_padron (Tu ejemplo: 1)
    1,                        -- Id_concepto (Tu ejemplo: 1)
    25,                       -- Monto (Ejemplo)
    3,                        -- Id_cobrador (Tu ejemplo: 3)
    datetime('now', 'localtime'), -- Fecha_cobro (Fecha y hora actual)
    'REC-2023-001',           -- Folio_Recibo (Debe ser único)
    'A',                      -- Estado (A=Activo)
    18.4567,                  -- Latitud (Opcional)
    -96.1234                  -- Longitud (Opcional)
);