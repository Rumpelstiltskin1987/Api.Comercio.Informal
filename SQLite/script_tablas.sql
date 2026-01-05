DROP TABLE IF EXISTS CobradorLog;
DROP TABLE IF EXISTS Cobrador;

CREATE TABLE Cobrador (
	Id_cobrador INTEGER PRIMARY KEY AUTOINCREMENT,
	Nombre TEXT NOT NULL,
	A_paterno TEXT NOT NULL,
	A_materno TEXT NOT NULL,
	Telefono TEXT,
	Email TEXT,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT
);

CREATE TABLE CobradorLog (
	Id_movimiento INTEGER NOT NULL,
	Id_cobrador INTEGER NOT NULL,
	Nombre TEXT NOT NULL,
	A_paterno TEXT NOT NULL,
	A_materno TEXT NOT NULL,
	Telefono TEXT,
	Email TEXT,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_cobrador),
	FOREIGN KEY (Id_cobrador) REFERENCES Cobrador(Id_cobrador)
);

DROP TABLE IF EXISTS ConceptoLog;
DROP TABLE IF EXISTS Concepto;

CREATE TABLE Concepto (
	Id_concepto INTEGER PRIMARY KEY AUTOINCREMENT,
	Descripcion TEXT NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT
);

CREATE TABLE ConceptoLog (
	Id_movimiento INTEGER NOT NULL,
	Id_concepto INTEGER NOT NULL ,
	Descripcion TEXT NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_concepto),
	FOREIGN KEY (Id_concepto) REFERENCES Concepto(Id_concepto)
);

DROP TABLE IF EXISTS Tarifa;

CREATE TABLE Tarifa (
    Id_tarifa INTEGER PRIMARY KEY AUTOINCREMENT,
    Id_concepto INTEGER NOT NULL,
    Id_gremio INTEGER,
    Monto REAL NOT NULL CHECK(Monto >= 0),
    Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
    Usuario_alta TEXT NOT NULL,
    Fecha_alta TEXT NOT NULL,
    Usuario_modificacion TEXT,
    Fecha_modificacion TEXT,
    FOREIGN KEY (Id_concepto) REFERENCES Concepto(Id_concepto),
    FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
);

DROP TABLE IF EXISTS TarifaLog;

CREATE TABLE TarifaLog (
    Id_movimiento INTEGER NOT NULL,
    Id_tarifa INTEGER NOT NULL,
    Id_concepto INTEGER NOT NULL,
    Id_gremio INTEGER,
    Monto REAL NOT NULL,
    Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
    Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
    Usuario_modificacion TEXT,
    Fecha_modificacion TEXT,
    PRIMARY KEY (Id_movimiento, Id_tarifa),
    FOREIGN KEY (Id_tarifa) REFERENCES Tarifa(Id_tarifa)
);

DROP TABLE IF EXISTS Folio;

CREATE TABLE Folio (
    Id_folio_serie INTEGER PRIMARY KEY AUTOINCREMENT,
    Id_gremio INTEGER, 
    Descripcion TEXT NOT NULL,
    Prefijo TEXT NOT NULL UNIQUE, 
    Siguiente_folio INTEGER NOT NULL DEFAULT 1 CHECK(Siguiente_Folio > 0),
    Anio_vigente INTEGER NOT NULL,    
    FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
);

DROP TABLE IF EXISTS MatriculaContador;

CREATE TABLE MatriculaContador (
    Tipo_vendedor TEXT NOT NULL CHECK(Tipo_Vendedor IN ('P', 'E')),
    Anio INTEGER NOT NULL,
    Siguiente_numero INTEGER NOT NULL DEFAULT 1 CHECK(Siguiente_Numero > 0),    
    PRIMARY KEY (Tipo_Vendedor, Anio)
);

DROP TABLE IF EXISTS Lider;

CREATE TABLE Lider (
	Id_lider INTEGER PRIMARY KEY AUTOINCREMENT,
	Nombre TEXT NOT NULL,
	A_paterno TEXT NOT NULL,
	A_materno TEXT NOT NULL,
	Telefono TEXT,
	Email TEXT,
	Direccion TEXT,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT
);

DROP TABLE IF EXISTS LiderLog;

CREATE TABLE LiderLog (
	Id_movimiento INTEGER NOT NULL,
	Id_lider INTEGER NOT NULL,
	Nombre TEXT NOT NULL,
	A_paterno TEXT NOT NULL,
	A_materno TEXT NOT NULL,
	Telefono TEXT,
	Email TEXT,
	Direccion TEXT,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_lider),
	FOREIGN KEY (Id_lider) REFERENCES Lider(Id_lider)
);

DROP TABLE IF EXISTS Gremio;

CREATE TABLE Gremio (
	Id_gremio INTEGER PRIMARY KEY AUTOINCREMENT,
	Descripcion TEXT NOT NULL,
	Id_lider INTEGER NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	FOREIGN KEY (Id_lider) REFERENCES Lider(Id_lider)
);

DROP TABLE IF EXISTS GremioLog;

CREATE TABLE GremioLog (
	Id_movimiento INTEGER NOT NULL,
	Id_gremio INTEGER NOT NULL,
	Descripcion TEXT NOT NULL,
	Id_lider INTEGER NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_gremio),
	FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
	FOREIGN KEY (Id_lider) REFERENCES Lider(Id_lider)	
);

DROP TABLE IF EXISTS Padron;

CREATE TABLE Padron (
	Id_padron INTEGER PRIMARY KEY AUTOINCREMENT,	
	Nombre TEXT NOT NULL,
	A_paterno TEXT,
	A_materno TEXT,
	Curp TEXT,
	Direccion TEXT,
	Telefono TEXT,
	Email TEXT,
	Matricula TEXT,
	Matricula_anterior TEXT,
	Id_gremio INTEGER,
    Tipo_Vendedor TEXT NOT NULL CHECK(Tipo_Vendedor IN ('P', 'E')), --P=Padron, E=Eventual    
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,	
    FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
);

DROP TABLE IF EXISTS PadronLog;

CREATE TABLE PadronLog (
	Id_movimiento INTEGER NOT NULL,
	Id_padron INTEGER NOT NULL,	
	Nombre TEXT NOT NULL,
	A_paterno TEXT,
	A_materno TEXT,
	Curp TEXT,
	Direccion TEXT,
	Telefono TEXT,
	Email TEXT,
	Matricula TEXT,
	Matricula_anterior TEXT,
	Id_gremio INTEGER,
    Tipo_Vendedor TEXT NOT NULL CHECK(Tipo_Vendedor IN ('P', 'E')),
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,    
	PRIMARY KEY (Id_movimiento, Id_padron),
	FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
);

DROP TABLE IF EXISTS UsuarioLog;

CREATE TABLE UsuarioLog (
	Id_movimiento	INTEGER NOT NULL,
	Id	INTEGER NOT NULL,
	UserName	TEXT NOT NULL,
	Nombre	TEXT NOT NULL,
	A_paterno	TEXT NOT NULL,
	A_materno	TEXT NOT NULL,
	Email	TEXT,
	PhoneNumber	TEXT,
	Rol TEXT NOT NULL,
	Estado	TEXT NOT NULL CHECK("Estado" IN ('A', 'I')),
	Tipo_movimiento	TEXT NOT NULL CHECK("Tipo_movimiento" IN ('A', 'B', 'M')),
	Usuario_modificacion	TEXT,
	Fecha_modificacion	TEXT,
	PRIMARY KEY("Id_movimiento","Id"),
	FOREIGN KEY("Id") REFERENCES "AspNetUsers"("Id")
);

DROP TABLE IF EXISTS Recaudacion;

CREATE TABLE Recaudacion (
	Id_recaudacion INTEGER PRIMARY KEY AUTOINCREMENT,
	Id_padron INTEGER NOT NULL,
	Id_concepto INTEGER NOT NULL,
	Monto REAL NOT NULL,
	Id INTEGER NOT NULL,
	Fecha_cobro TEXT NOT NULL,
    Folio_Recibo TEXT NOT NULL UNIQUE,
    Estado TEXT NOT NULL DEFAULT 'A' CHECK(Estado IN ('A', 'C')), -- A=Activo, C=Cancelado
    Latitud REAL,
    Longitud REAL,	
	FOREIGN KEY (Id_padron) REFERENCES Padron(Id_padron),
	FOREIGN KEY (Id_concepto) REFERENCES Concepto(Id_concepto),
	FOREIGN KEY (Id) REFERENCES AspNetUsers(Id)
);


