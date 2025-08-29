CREATE TABLE "Categoria" (
	"Id_Categoria"	INTEGER,
	"Descripcion"	TEXT NOT NULL,
	"Estado"	TEXT NOT NULL CHECK(Status IN('A','I')),
	"Usuario_alta"	TEXT NOT NULL,
	"Fecha_alta"	TEXT NOT NULL,
	"Usuario_modificacion"	TEXT,
	"Fecha_modificacion"	TEXT,
	PRIMARY KEY("Id_Categoria" AUTOINCREMENT)
);

