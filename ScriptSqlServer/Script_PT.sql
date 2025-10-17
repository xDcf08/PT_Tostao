-- Verificar si la base de datos existe; si no, la crea.
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GestionDocumentalDB')
BEGIN
    CREATE DATABASE GestionDocumentalDB;
END
GO

-- Usar la base de datos recién creada o existente.
USE GestionDocumentalDB;
GO

-- ========= 1. DEFINICIÓN DE LA TABLA (DDL) - CORREGIDO =========
-- Eliminar la tabla si ya existe para asegurar un inicio limpio.
IF OBJECT_ID('dbo.Documentos', 'U') IS NOT NULL
    DROP TABLE dbo.Documentos;
GO

-- Crear la tabla 'Documentos'
CREATE TABLE dbo.Documentos (
    ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), -- Se generará un nuevo GUID automáticamente en cada inserción
    Titulo NVARCHAR(255) NOT NULL,
    Autor NVARCHAR(150) NOT NULL,
    Tipo NVARCHAR(100) NOT NULL,
    Estado NVARCHAR(50) NOT NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

PRINT 'Tabla "Documentos" creada exitosamente con ID de tipo GUID.';
GO

-- ========= 2. CREACIÓN DEL STORED PROCEDURE =========
IF OBJECT_ID('dbo.SP_ReportePorAutorYTipo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_ReportePorAutorYTipo;
GO

CREATE PROCEDURE dbo.SP_ReportePorAutorYTipo
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ISNULL(Autor, 'TOTAL GENERAL') AS Autor,
        CASE
            WHEN Autor IS NOT NULL THEN ISNULL(Tipo, 'Todos')
            ELSE ''
        END AS Tipo,
        COUNT(ID) AS CantidadDocumentos
    FROM
        dbo.Documentos
    GROUP BY
        ROLLUP (Autor, Tipo)
    ORDER BY
        Autor, Tipo;
END
GO

PRINT 'Stored Procedure "SP_ReportePorAutorYTipo" creado exitosamente.';
GO

-- =================================================================
-- SCRIPT DE INSERCIÓN DE DATOS DE PRUEBA (SEED DATA)
-- Base de Datos: GestionDocumentalDB
-- =================================================================

USE GestionDocumentalDB;
GO

-- Limpiar la tabla antes de insertar nuevos datos
DELETE FROM dbo.Documentos;
GO

-- Insertar 25 documentos de prueba. El ID se generará automáticamente.
INSERT INTO dbo.Documentos (Titulo, Autor, Tipo, Estado, FechaRegistro) VALUES
-- Documentos recientes (últimos 90 días desde Oct 2025)
('Análisis de Mercado Q3 2025', 'Ana Torres', 'Informe', 'Registrado', '2025-10-01'),
('Propuesta Comercial - Acme Corp', 'Carlos Ruiz', 'Contrato', 'Pendiente', '2025-09-15'),
('Reunión de Kick-off Proyecto Alfa', 'Luisa Fernandez', 'Acta', 'Validado', '2025-09-05'),
('Reporte de Ventas Septiembre', 'Ana Torres', 'Informe', 'Validado', '2025-10-02'),
('Acuerdo de Confidencialidad', 'Sofia Gomez', 'Contrato', 'Registrado', '2025-08-20'),
('Minuta Reunión Semanal', 'Carlos Ruiz', 'Acta', 'Pendiente', '2025-10-10'),
('Especificaciones Técnicas v1.1', 'David Lopez', 'Informe', 'Validado', '2025-09-22'),
('Contrato de Arrendamiento Oficina', 'Sofia Gomez', 'Contrato', 'Archivado', '2025-08-01'),
('Resumen Ejecutivo Proyecto Beta', 'Ana Torres', 'Informe', 'Pendiente', '2025-10-05'),
('Acta de Entrega Final', 'Luisa Fernandez', 'Acta', 'Validado', '2025-09-30'),

-- Documentos antiguos (más de 90 días en estado 'Pendiente' para probar la tarea automática)
('Investigación de Competencia', 'Carlos Ruiz', 'Informe', 'Pendiente', '2025-05-10'),
('Borrador Contrato Proveedor X', 'Sofia Gomez', 'Contrato', 'Pendiente', '2025-04-20'),
('Acta de Comité Directivo', 'Luisa Fernandez', 'Acta', 'Pendiente', '2025-06-01'),
('Análisis de Riesgos Inicial', 'David Lopez', 'Informe', 'Pendiente', '2025-03-15'),
('Contrato de Servicios Profesionales', 'Sofia Gomez', 'Contrato', 'Pendiente', '2025-02-11'),

-- Documentos ya archivados o en otros estados
('Informe Anual 2024', 'Ana Torres', 'Informe', 'Archivado', '2025-01-15'),
('Contrato Marco con Cliente Y', 'Carlos Ruiz', 'Contrato', 'Archivado', '2024-11-20'),
('Acta de Constitución', 'Luisa Fernandez', 'Acta', 'Archivado', '2024-01-30'),
('Reporte Financiero Q1 2025', 'David Lopez', 'Informe', 'Validado', '2025-04-05'),
('Contrato de Mantenimiento', 'Sofia Gomez', 'Contrato', 'Validado', '2025-07-18'),
('Revisión de Hitos Q2', 'Ana Torres', 'Informe', 'Archivado', '2025-07-02'),
('Acuerdo de Nivel de Servicio', 'Carlos Ruiz', 'Contrato', 'Pendiente', '2025-09-18'),
('Acta de Cierre de Proyecto Gamma', 'Luisa Fernandez', 'Acta', 'Archivado', '2025-06-25'),
('Plan de Marketing 2025', 'David Lopez', 'Informe', 'Validado', '2025-02-28'),
('Análisis de Satisfacción del Cliente', 'Ana Torres', 'Informe', 'Registrado', '2025-10-14');
GO

PRINT '25 registros de prueba insertados en la tabla "Documentos".';
GO

-- =================================================================
-- SCRIPT DE OPTIMIZACIÓN Y QUERIES COMPLEJAS
-- Base de Datos: GestionDocumentalDB
-- =================================================================

USE GestionDocumentalDB;
GO

-- ========= 1. QUERY COMPLEJA: PROMEDIO DE DÍAS ENTRE ESTADOS =========
/*
  Requerimiento: Calcular el promedio de días entre el estado "Registrado" y "Validado",
  agrupado por Tipo de documento.

  Estrategia: Para hacer este cálculo, necesitamos registrar la fecha en que un documento
  es validado. Añadiremos una nueva columna a la tabla `Documentos` llamada `FechaValidado`
  que permitirá calcular la diferencia de días (DATEDIFF) de forma sencilla y eficiente.
*/

-- Paso 1.1: Añadir la nueva columna a la tabla (si no existe)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'FechaValidado' AND Object_ID = Object_ID(N'dbo.Documentos'))
BEGIN
    ALTER TABLE dbo.Documentos
    ADD FechaValidado DATETIME2 NULL;
    PRINT 'Columna "FechaValidado" añadida a la tabla "Documentos".';
END
GO

-- Paso 1.2: Simular la actualización de algunos documentos para tener datos de prueba.
-- (En la aplicación real, esto lo haría el microservicio de Laravel).
UPDATE Documentos SET FechaValidado = DATEADD(day, 10, FechaRegistro) WHERE Tipo = 'Informe' AND Estado = 'Validado';
UPDATE Documentos SET FechaValidado = DATEADD(day, 25, FechaRegistro) WHERE Tipo = 'Acta' AND Estado = 'Validado';
UPDATE Documentos SET FechaValidado = DATEADD(day, 40, FechaRegistro) WHERE Tipo = 'Contrato' AND Estado = 'Validado';
GO

-- Paso 1.3: La query optimizada para el reporte
PRINT 'Ejecutando query para calcular promedio de días de validación por Tipo...';
SELECT
    Tipo,
    AVG(DATEDIFF(day, FechaRegistro, FechaValidado)) AS PromedioDiasValidacion
FROM
    dbo.Documentos
WHERE
    Estado = 'Validado' AND FechaValidado IS NOT NULL
GROUP BY
    Tipo
ORDER BY
    Tipo;
GO


-- ========= 2. CREACIÓN DE ÍNDICES PARA OPTIMIZACIÓN =========
/*
  Requerimiento: Crear índices para optimizar las consultas, especialmente la búsqueda avanzada.

  Estrategia: La búsqueda avanzada filtra por `Autor`, `Tipo` y `Estado`.
  Crearemos un índice no agrupado (non-clustered index) que cubra estas tres
  columnas. Esto permitirá a SQL Server encontrar los resultados de la búsqueda
  de forma extremadamente rápida sin tener que escanear toda la tabla.
*/

PRINT 'Creando índice optimizado para búsquedas...';

-- Crear un índice compuesto en las columnas más usadas para filtrar.
-- Se incluye 'Titulo' y 'FechaRegistro' para que la consulta no necesite
-- acceder a la tabla principal (covering index).
CREATE NONCLUSTERED INDEX IX_Documentos_BusquedaAvanzada
ON dbo.Documentos (Estado, Tipo, Autor)
INCLUDE (Titulo, FechaRegistro);
GO

PRINT 'Índice "IX_Documentos_BusquedaAvanzada" creado exitosamente.';
GO