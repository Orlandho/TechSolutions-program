-- ====================================================================================
-- SCRIPT DE DATOS DE PRUEBA PARA TECHSOLUTIONS
-- Compatible con el modelo de Entity Framework Core
-- ====================================================================================

USE TechSolutionsDB;
GO

-- 1. CLIENTES
PRINT 'Insertando clientes de prueba...';
INSERT INTO Clientes (RazonSocial, RUC, EmailContacto, Telefono, Direccion) VALUES 
('Minera Yanacocha S.R.L.', '20134567891', 'compras@yanacocha.pe', '01-555-1234', 'Av. La Paz 104, Cajamarca'),
('Banco de Crédito del Perú', '20100047218', 'proyectos@bcp.com.pe', '01-313-2000', 'Centenario 156, La Molina'),
('Supermercados Peruanos S.A.', '20100070970', 'ti@spsa.com.pe', '01-614-5000', 'Morelli 181, San Borja');
GO

-- 2. PROYECTOS
PRINT 'Insertando proyectos de prueba...';
-- Proyecto para la Minera
INSERT INTO Proyectos (Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('Sistema de Gestión de Activos Mineros', 'Migración de sistema legacy a nube Azure.', 1, GETDATE(), DATEADD(MONTH, 6, GETDATE()), 150000.00, 'En Desarrollo', 'Alta');

-- Proyecto para el Banco
INSERT INTO Proyectos (Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('App Móvil Yape 3.0', 'Nuevas funcionalidades de QR.', 2, DATEADD(MONTH, -1, GETDATE()), DATEADD(MONTH, 2, GETDATE()), 85000.50, 'Pruebas', 'Alta');

-- Proyecto para Supermercados
INSERT INTO Proyectos (Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) VALUES 
('Intranet de RRHH', 'Portal para boletas de pago.', 3, GETDATE(), DATEADD(MONTH, 3, GETDATE()), 25000.00, 'Planificación', 'Media');
GO

-- 3. TAREAS
PRINT 'Insertando tareas de prueba...';
-- Tareas del Proyecto Minero (ID 1)
INSERT INTO Tareas (Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
('Diseñar Arquitectura en Azure', 1, 'Finalizado', 'Alta', 'user-guid-1', DATEADD(DAY, -5, GETDATE())),
('Configurar VPN Site-to-Site', 1, 'En Progreso', 'Alta', 'user-guid-2', DATEADD(DAY, 5, GETDATE())),
('Desarrollar API REST de Activos', 1, 'Pendiente', 'Media', 'user-guid-1', DATEADD(DAY, 15, GETDATE()));

-- Tareas del Proyecto Banco (ID 2)
INSERT INTO Tareas (Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
('Pruebas de carga (Load Testing)', 2, 'En Progreso', 'Alta', 'user-guid-3', GETDATE()),
('Corregir bug en Login biométrico', 2, 'Pendiente', 'Alta', 'user-guid-2', DATEADD(DAY, 2, GETDATE()));
GO

PRINT 'Datos de prueba insertados exitosamente.';
GO

-- Verificar los datos insertados
PRINT 'Verificando datos insertados...';
SELECT COUNT(*) AS TotalClientes FROM Clientes;
SELECT COUNT(*) AS TotalProyectos FROM Proyectos;
SELECT COUNT(*) AS TotalTareas FROM Tareas;
GO
