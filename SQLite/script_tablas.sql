CREATE TABLE Categoria (
	Id_categoria INTEGER PRIMARY KEY AUTOINCREMENT,
	Nombre TEXT NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT
);


CREATE TABLE CategoriaLog (
	Id_movimiento INTEGER NOT NULL,
	Id_categoria INTEGER NOT NULL ,
	Nombre TEXT NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_categoria),
	FOREIGN KEY (Id_categoria) REFERENCES Categoria(Id_categoria)
);

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

CREATE TABLE Cuota (
	Id_cuota INTEGER PRIMARY KEY AUTOINCREMENT,
	Monto REAL NOT NULL CHECK(Monto >= 0),
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT
);

CREATE TABLE CuotaLog (
	Id_movimiento INTEGER NOT NULL,
	Id_cuota INTEGER NOT NULL ,
	Descripcion TEXT NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_cuota),
	FOREIGN KEY (Id_cuota) REFERENCES Cuota(Id_cuota)
);

CREATE TABLE Folio (
	Id_folio INTEGER PRIMARY KEY AUTOINCREMENT,
	N_folio INTEGER NOT NULL,
	Fecha TEXT NOT NULL
);

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

CREATE TABLE LiderlLog (
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

CREATE TABLE Padron (
	Id_padron INTEGER PRIMARY KEY AUTOINCREMENT,
	Matricula TEXT NOT NULL,
	Nombre TEXT NOT NULL,
	A_paterno TEXT NOT NULL,
	A_materno TEXT NOT NULL,
	Curp TEXT,
	Direccion TEXT,
	Telefono TEXT,
	Email TEXT,
	Id_gremio INTEGER NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
);

CREATE TABLE PadronLog (
	Id_movimiento INTEGER NOT NULL,
	Id_padron INTEGER NOT NULL,
	Matricula TEXT NOT NULL,
	Nombre TEXT NOT NULL,
	A_paterno TEXT NOT NULL,
	A_materno TEXT NOT NULL,
	Curp TEXT,
	Direccion TEXT,
	Telefono TEXT,
	Email TEXT,
	Id_gremio INTEGER NOT NULL,
	Estado TEXT NOT NULL CHECK(Estado IN ('A', 'I')),
	Tipo_movimiento TEXT NOT NULL CHECK(Tipo_movimiento IN ('A','B','M')),
	Usuario_alta TEXT NOT NULL,
	Fecha_alta TEXT NOT NULL,
	Usuario_modificacion TEXT,
	Fecha_modificacion TEXT,
	PRIMARY KEY (Id_movimiento, Id_padron),
	FOREIGN KEY (Id_gremio) REFERENCES Gremio(Id_gremio)
);

CREATE TABLE Recaudacion (
	Id_recaudacion INTEGER PRIMARY KEY AUTOINCREMENT,
	Id_padron INTEGER NOT NULL,
	Id_concepto INTEGER NOT NULL,
	Monto REAL NOT NULL,
	Id_cobrador INTEGER NOT NULL,
	Fecha_cobro TEXT NOT NULL,	
	FOREIGN KEY (Id_padron) REFERENCES Padron(Id_padron),
	FOREIGN KEY (Id_concepto) REFERENCES Concepto(Id_concepto),
	FOREIGN KEY (Id_cobrador) REFERENCES Cobrador(Id_cobrador)
);


