CREATE TABLE dbo.Alumno (
Id INT IDENTITY (1,1) PRIMARY KEY,
Codigo NVARCHAR(20) NOT NULL UNIQUE,
Nombre NVARCHAR(100) NOT NULL,
Ciclo INT,
Curso INT,
Estado BIT DEFAULT 1,
Version RowVersion
)
GO

CREATE TABLE dbo.Calificacion(
Id INT IDENTITY (1,1) PRIMARY KEY,
IdAlumno INT REFERENCES dbo.Alumno(Id) NOT NULL,
IdTipoEvaluacion INT NOT NULL,
Nota DECIMAL(4,2) NULL,
)
GO

INSERT INTO dbo.Alumno
(
    Codigo,
    Nombre,
    Ciclo,
    Curso,
    Estado
)
VALUES
('I201810001', 'Jorge Vargas Ipince', 3, 1, 1 ),
('I201810002', 'Miguel Cornejo', 4, 2, 1 ),
('I201810003', 'Mark Zuckemberg', 5, 3, 1 ),
('I201810004', 'Bill Gates', 6, 4, 1 )
GO

INSERT INTO dbo.Calificacion
(
    IdAlumno,
    IdTipoEvaluacion,
    Nota
)
VALUES
(  1, 1, 14.5 ),
(  1, 2, 15.0 ),
(  1, 3, 16.5 ),
(  1, 4, 17.0 ),
(  1, 5, 18.5 ),
(  2, 1, 17.0 ),
(  2, 2, 16.5 ),
(  2, 3, 15.0 ),
(  2, 4, 14.5 ),
(  2, 5, 13.0 ),
(  3, 1, 12.5 ),
(  3, 2, 13.0 ),
(  3, 3, 14.5 ),
(  3, 4, 15.0 ),
(  3, 5, 16.5 ),
(  4, 1, 17.0 ),
(  4, 2, 15.5 ),
(  4, 3, 12.0 ),
(  4, 4, 11.5 ),
(  4, 5, NULL )


SELECT * FROM dbo.Alumno
SELECT * FROM dbo.Calificacion