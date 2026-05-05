CREATE INDEX IX_Padron ON Padron(Id_padron);
CREATE INDEX IX_Padron_Curp ON Padron(Curp);
CREATE INDEX IX_Padron_Matricula ON Padron(Matricula);
CREATE INDEX IX_Padron_Gremio ON Padron(Id_gremio);
CREATE INDEX IX_Padron_Nombre_Completo ON Padron(A_paterno, A_materno, Nombre);
CREATE INDEX IX_Padron_Estado_Tipo ON Padron(Estado, Tipo_Vendedor);