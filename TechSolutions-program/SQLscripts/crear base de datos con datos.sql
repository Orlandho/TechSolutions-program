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
-- SECCIÓN 1: TABLAS DEL NEGOCIO (DDL)
-- ====================================================================================

-- Eliminar tablas en orden correcto (de dependiente a principal)
IF OBJECT_ID('Tareas', 'U') IS NOT NULL DROP TABLE Tareas;
IF OBJECT_ID('Proyectos', 'U') IS NOT NULL DROP TABLE Proyectos;
IF OBJECT_ID('Clientes', 'U') IS NOT NULL DROP TABLE Clientes;
IF OBJECT_ID('HistorialReportes', 'U') IS NOT NULL DROP TABLE HistorialReportes;
GO

-- TABLA: CLIENTES
-- Contexto: Caso Integrador "Desarrollo para diversas empresas".
CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RazonSocial NVARCHAR(150) NOT NULL,
    RUC CHAR(11) NOT NULL UNIQUE, -- Validación estricta de RUC
    Direccion NVARCHAR(200),
    EmailContacto NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- TABLA: PROYECTOS
-- Contexto: CU-01 Registrar Proyecto.
CREATE TABLE Proyectos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    ClienteId INT NOT NULL, -- FK a Clientes
    
    FechaInicio DATETIME NOT NULL,
    FechaFinEstimada DATETIME,
    
    -- Restricción: Presupuesto no negativo (Regla de Negocio)
    Presupuesto DECIMAL(18, 2) NOT NULL CHECK (Presupuesto >= 0),
    
    -- Estados válidos según el documento
    Estado NVARCHAR(20) DEFAULT 'Planificacion' CHECK (Estado IN ('Planificacion', 'En Desarrollo', 'Pruebas', 'Terminado')),
    Prioridad NVARCHAR(10) DEFAULT 'Media',
    
    CONSTRAINT FK_Proyectos_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
GO

-- TABLA: TAREAS
-- Contexto: CU-02 y CU-03 (Seguimiento Operativo).
CREATE TABLE Tareas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(200) NOT NULL,
    ProyectoId INT NOT NULL, -- FK a Proyectos
    
    -- ID del Usuario Responsable (NVARCHAR(450) es el estándar de ASP.NET Identity)
    ResponsableId NVARCHAR(450), 
    
    FechaLimite DATETIME,
    
    Estado NVARCHAR(20) DEFAULT 'Pendiente' CHECK (Estado IN ('Pendiente', 'En Progreso', 'Bloqueado', 'Finalizado')),
    Prioridad NVARCHAR(10) DEFAULT 'Media',
    
    CONSTRAINT FK_Tareas_Proyectos FOREIGN KEY (ProyectoId) REFERENCES Proyectos(Id) ON DELETE CASCADE
);
GO

-- TABLA: HISTORIAL DE REPORTES
-- Contexto: Auditoría del Patrón Strategy.
CREATE TABLE HistorialReportes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TituloReporte NVARCHAR(100),
    TipoFormato NVARCHAR(10) CHECK (TipoFormato IN ('PDF', 'Excel')),
    FechaGeneracion DATETIME DEFAULT GETDATE(),
    UsuarioGenerador NVARCHAR(100)
);
GO

-- ====================================================================================
-- SECCIÓN 2: DATOS DE PRUEBA (SEED DATA - DML)
-- ====================================================================================
-- Insertamos datos ficticios para poder probar el Dashboard y los Reportes inmediatamente.

PRINT 'Insertando datos de prueba...'

-- 1. CLIENTES
INSERT INTO Clientes (RazonSocial, RUC, EmailContacto, Telefono, Direccion) VALUES 
('Minera Yanacocha S.R.L.', '20134567891', 'compras@yanacocha.pe', '01-555-1234', 'Av. La Paz 104, Cajamarca'),
('Banco de Crédito del Perú', '20100047218', 'proyectos@bcp.com.pe', '01-313-2000', 'Centenario 156, La Molina'),
('Supermercados Peruanos S.A.', '20100070970', 'ti@spsa.com.pe', '01-614-5000', 'Morelli 181, San Borja');

-- 2. PROYECTOS
-- Proyecto para la Minera
INSERT INTO Proyectos (Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('Sistema de Gestión de Activos Mineros', 'Migración de sistema legacy a nube Azure.', 1, GETDATE(), DATEADD(MONTH, 6, GETDATE()), 150000.00, 'En Desarrollo', 'Alta');

-- Proyecto para el Banco
INSERT INTO Proyectos (Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('App Móvil Yape 3.0', 'Nuevas funcionalidades de QR.', 2, DATEADD(MONTH, -1, GETDATE()), DATEADD(MONTH, 2, GETDATE()), 85000.50, 'Pruebas', 'Alta');

-- Proyecto para Supermercados
INSERT INTO Proyectos (Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('Intranet de RRHH', 'Portal para boletas de pago.', 3, GETDATE(), DATEADD(MONTH, 3, GETDATE()), 25000.00, 'Planificacion', 'Media');

-- 3. TAREAS
-- Tareas del Proyecto Minero (ID 1)
INSERT INTO Tareas (Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
('Diseñar Arquitectura en Azure', 1, 'Finalizado', 'Alta', 'user-guid-1', DATEADD(DAY, -5, GETDATE())),
('Configurar VPN Site-to-Site', 1, 'En Progreso', 'Alta', 'user-guid-2', DATEADD(DAY, 5, GETDATE())),
('Desarrollar API REST de Activos', 1, 'Pendiente', 'Media', 'user-guid-1', DATEADD(DAY, 15, GETDATE()));

-- Tareas del Proyecto Banco (ID 2)
INSERT INTO Tareas (Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
('Pruebas de carga (Load Testing)', 2, 'En Progreso', 'Critica', 'user-guid-3', GETDATE()),
('Corregir bug en Login biométrico', 2, 'Pendiente', 'Alta', 'user-guid-2', DATEADD(DAY, 2, GETDATE()));

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