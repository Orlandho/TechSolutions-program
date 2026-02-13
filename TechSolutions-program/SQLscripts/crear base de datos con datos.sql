--Base de datos de TechSolutions SAC
/*
 * ====================================================================================
 * SCRIPT DE BASE DE DATOS: TechSolutionsDB
 * ====================================================================================
 * PROYECTO: Sistema de Gestión de Proyectos TechSolutions
 * ARQUITECTURA: Relacional (SQL Server)
 * FECHA: 2026-02-##
 * ====================================================================================
 */

-- 1. Creación de la Base de Datos
-- Verificamos si existe para no dar error, si no existe la crea.
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'TechSolutionsDB')
BEGIN
    CREATE DATABASE TechSolutionsDB;
END
GO

USE TechSolutionsDB;
GO

-- ====================================================================================
-- SECCIÓN 0: ROLES DE SEGURIDAD (ASP.NET Core Identity)
-- ====================================================================================
-- Inserta roles solo si existen las tablas de Identity (creadas por migraciones)
IF OBJECT_ID('AspNetRoles', 'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE NormalizedName = 'LIDER')
        INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (CONVERT(NVARCHAR(450), NEWID()), 'Lider', 'LIDER');

    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE NormalizedName = 'DESARROLLADOR')
        INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (CONVERT(NVARCHAR(450), NEWID()), 'Desarrollador', 'DESARROLLADOR');

    IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE NormalizedName = 'ADMINISTRADOR')
        INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (CONVERT(NVARCHAR(450), NEWID()), 'Administrador', 'ADMINISTRADOR');
END
GO

-- ====================================================================================
-- SECCIÓN 1: TABLAS DEL NEGOCIO (DDL)
-- ====================================================================================

-- TABLA: CLIENTES
-- Contexto: Caso Integrador "Desarrollo para diversas empresas".
IF OBJECT_ID('Clientes', 'U') IS NOT NULL DROP TABLE Clientes;
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial NVARCHAR(100) NOT NULL,
    RUC CHAR(11) NOT NULL UNIQUE, -- Validación estricta de RUC
    CorreoContacto NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(200)
);
GO

-- TABLA: PROYECTOS
-- Contexto: CU-01 Registrar Proyecto.
IF OBJECT_ID('Proyectos', 'U') IS NOT NULL DROP TABLE Proyectos;
CREATE TABLE Proyectos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Cliente NVARCHAR(150) NOT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFinEstimada DATETIME,
    Presupuesto DECIMAL(18, 2) NOT NULL CHECK (Presupuesto >= 0),
    Estado NVARCHAR(20) DEFAULT 'Planificación' NOT NULL,
    Prioridad NVARCHAR(10) DEFAULT 'Media' NOT NULL
);
GO

-- TABLA: TAREAS
-- Contexto: CU-02 y CU-03 (Seguimiento Operativo).
IF OBJECT_ID('Tareas', 'U') IS NOT NULL DROP TABLE Tareas;
CREATE TABLE Tareas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(200) NOT NULL,
    Estado NVARCHAR(20) DEFAULT 'Pendiente' NOT NULL,
    Prioridad NVARCHAR(10) DEFAULT 'Media' NOT NULL,
    Responsable NVARCHAR(200),
    FechaLimite DATETIME,
    ProyectoId INT NOT NULL,
    CONSTRAINT FK_Tareas_Proyectos FOREIGN KEY (ProyectoId) REFERENCES Proyectos(Id) ON DELETE CASCADE
);
GO

-- TABLA: REPORTES
-- Contexto: Auditoría del Patrón Strategy.
IF OBJECT_ID('Reportes', 'U') IS NOT NULL DROP TABLE Reportes;
CREATE TABLE Reportes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(100) NOT NULL,
    TipoFormato NVARCHAR(10) NOT NULL,
    FechaGeneracion DATETIME DEFAULT GETDATE(),
    GeneradoPor NVARCHAR(100)
);
GO

-- ====================================================================================
-- SECCIÓN 2: DATOS DE PRUEBA (SEED DATA - DML)
-- ====================================================================================
-- Insertamos datos ficticios para poder probar el Dashboard y los Reportes inmediatamente.

PRINT 'Insertando datos de prueba...'

-- 1. CLIENTES
INSERT INTO Clientes (RazonSocial, RUC, CorreoContacto, Telefono, Direccion) VALUES 
('Minera Yanacocha S.R.L.', '20134567891', 'compras@yanacocha.pe', '01-555-1234', 'Av. La Paz 104, Cajamarca'),
('Banco de Crédito del Perú', '20100047218', 'proyectos@bcp.com.pe', '01-313-2000', 'Centenario 156, La Molina'),
('Supermercados Peruanos S.A.', '20100070970', 'ti@spsa.com.pe', '01-614-5000', 'Morelli 181, San Borja');

-- 2. PROYECTOS
INSERT INTO Proyectos (Nombre, Cliente, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('Sistema de Gestión de Activos Mineros', 'Minera Yanacocha S.R.L.', GETDATE(), DATEADD(MONTH, 6, GETDATE()), 150000.00, 'En Desarrollo', 'Alta');

INSERT INTO Proyectos (Nombre, Cliente, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('App Móvil Yape 3.0', 'Banco de Crédito del Perú', DATEADD(MONTH, -1, GETDATE()), DATEADD(MONTH, 2, GETDATE()), 85000.50, 'Pruebas', 'Alta');

INSERT INTO Proyectos (Nombre, Cliente, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('Intranet de RRHH', 'Supermercados Peruanos S.A.', GETDATE(), DATEADD(MONTH, 3, GETDATE()), 25000.00, 'Planificación', 'Media');

-- 3. TAREAS
INSERT INTO Tareas (Descripcion, Estado, Prioridad, Responsable, FechaLimite, ProyectoId) VALUES 
('Diseñar Arquitectura en Azure', 'Finalizado', 'Alta', 'user-guid-1', DATEADD(DAY, -5, GETDATE()), 1),
('Configurar VPN Site-to-Site', 'En Progreso', 'Alta', 'user-guid-2', DATEADD(DAY, 5, GETDATE()), 1),
('Desarrollar API REST de Activos', 'Pendiente', 'Media', 'user-guid-1', DATEADD(DAY, 15, GETDATE()), 1);

INSERT INTO Tareas (Descripcion, Estado, Prioridad, Responsable, FechaLimite, ProyectoId) VALUES 
('Pruebas de carga (Load Testing)', 'En Progreso', 'Critica', 'user-guid-3', GETDATE(), 2),
('Corregir bug en Login biométrico', 'Pendiente', 'Alta', 'user-guid-2', DATEADD(DAY, 2, GETDATE()), 2);

PRINT 'Base de datos creada y poblada exitosamente.'
GO

-- ====================================================================================
-- SECCIÓN 3: DESTRUCCIÓN (PELIGRO - SOLO PARA REINICIAR)
-- ====================================================================================
/*
    IMPORTANTE: Descomenta las siguientes líneas SOLO si quieres borrar 
    toda la base de datos y empezar de cero. Esto borrará todos los datos.
*/

/*
USE master;
GO
ALTER DATABASE TechSolutionsDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE TechSolutionsDB;
GO
PRINT 'Base de datos TechSolutionsDB eliminada.';
*/