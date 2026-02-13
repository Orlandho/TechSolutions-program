-- ====================================================================================
-- SCRIPT DE DATOS DE PRUEBA COMPLETOS PARA TECHSOLUTIONS
-- Compatible con el modelo de Entity Framework Core
-- Usa los IDs reales de usuarios de AspNetUsers
-- ====================================================================================

USE TechSolutionsDB;
GO

PRINT 'Iniciando inserción de datos de prueba...';
GO

-- ====================================================================================
-- PASO 1: OBTENER IDs DE USUARIOS EXISTENTES
-- ====================================================================================
DECLARE @AdminId NVARCHAR(450);
DECLARE @LiderId NVARCHAR(450);
DECLARE @DesarrolladorId NVARCHAR(450);

SELECT @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@techsolutions.com';
SELECT @LiderId = Id FROM AspNetUsers WHERE Email = 'lider@techsolutions.com';
SELECT @DesarrolladorId = Id FROM AspNetUsers WHERE Email = 'desarrollador@techsolutions.com';

IF @AdminId IS NULL OR @LiderId IS NULL OR @DesarrolladorId IS NULL
BEGIN
    PRINT 'ERROR: No se encontraron los usuarios. Asegúrate de que la aplicación se haya ejecutado al menos una vez.';
    RETURN;
END

PRINT 'Usuarios encontrados:';
PRINT '  Admin: ' + @AdminId;
PRINT '  Líder: ' + @LiderId;
PRINT '  Desarrollador: ' + @DesarrolladorId;
GO

-- ====================================================================================
-- PASO 2: LIMPIAR DATOS EXISTENTES (OPCIONAL)
-- ====================================================================================
-- Descomenta estas líneas si quieres limpiar datos anteriores
/*
DELETE FROM Tareas;
DELETE FROM Proyectos;
DELETE FROM Clientes;
DELETE FROM Reportes;
DBCC CHECKIDENT ('Tareas', RESEED, 0);
DBCC CHECKIDENT ('Proyectos', RESEED, 0);
DBCC CHECKIDENT ('Clientes', RESEED, 0);
DBCC CHECKIDENT ('Reportes', RESEED, 0);
PRINT 'Datos anteriores eliminados.';
*/

-- ====================================================================================
-- PASO 3: INSERTAR CLIENTES
-- ====================================================================================
PRINT 'Insertando clientes...';

SET IDENTITY_INSERT Clientes ON;

INSERT INTO Clientes (Id, RazonSocial, RUC, EmailContacto, Telefono, Direccion) VALUES 
(1, 'Minera Yanacocha S.R.L.', '20134567891', 'compras@yanacocha.pe', '076-582000', 'Av. La Paz 104, Cajamarca'),
(2, 'Banco de Crédito del Perú', '20100047218', 'proyectos@bcp.com.pe', '01-313-2000', 'Centenario 156, La Molina, Lima'),
(3, 'Supermercados Peruanos S.A.', '20100070970', 'ti@spsa.com.pe', '01-614-5000', 'Morelli 181, San Borja, Lima'),
(4, 'Telefónica del Perú S.A.A.', '20100017491', 'sistemas@telefonica.pe', '01-611-9000', 'Av. Arequipa 1155, Lima'),
(5, 'Southern Copper Corporation', '20100113612', 'contacto@southernperu.com', '054-599300', 'Av. Caminos del Inca 171, Surco');

SET IDENTITY_INSERT Clientes OFF;

PRINT '✓ 5 clientes insertados';
GO

-- ====================================================================================
-- PASO 4: INSERTAR PROYECTOS
-- ====================================================================================
PRINT 'Insertando proyectos...';

-- Obtener IDs de usuarios para asignación
DECLARE @AdminId NVARCHAR(450);
DECLARE @LiderId NVARCHAR(450);
DECLARE @DesarrolladorId NVARCHAR(450);

SELECT @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@techsolutions.com';
SELECT @LiderId = Id FROM AspNetUsers WHERE Email = 'lider@techsolutions.com';
SELECT @DesarrolladorId = Id FROM AspNetUsers WHERE Email = 'desarrollador@techsolutions.com';

SET IDENTITY_INSERT Proyectos ON;

-- Proyecto 1: Minera (En Desarrollo)
INSERT INTO Proyectos (Id, Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) 
VALUES (1, 'Sistema de Gestión de Activos Mineros', 
        'Migración completa del sistema legacy a nube Azure con integración IoT para monitoreo en tiempo real de maquinaria pesada.', 
        1, DATEADD(MONTH, -2, GETDATE()), DATEADD(MONTH, 4, GETDATE()), 150000.00, 'En Desarrollo', 'Alta');

-- Proyecto 2: Banco BCP (En Pruebas)
INSERT INTO Proyectos (Id, Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) 
VALUES (2, 'App Móvil Yape 3.0', 
        'Desarrollo de nuevas funcionalidades: pagos con QR dinámico, transferencias internacionales y programa de puntos.', 
        2, DATEADD(MONTH, -3, GETDATE()), DATEADD(MONTH, 1, GETDATE()), 85000.50, 'Pruebas', 'Alta');

-- Proyecto 3: Supermercados (Planificación)
INSERT INTO Proyectos (Id, Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) 
VALUES (3, 'Intranet de RRHH', 
        'Portal web para gestión de recursos humanos: boletas de pago electrónicas, solicitudes de vacaciones y capacitaciones.', 
        3, GETDATE(), DATEADD(MONTH, 3, GETDATE()), 25000.00, 'Planificación', 'Media');

-- Proyecto 4: Telefónica (En Desarrollo)
INSERT INTO Proyectos (Id, Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) 
VALUES (4, 'Sistema de Atención al Cliente Omnicanal', 
        'Plataforma unificada para gestión de tickets de soporte por web, app móvil, chat y teléfono con IA.', 
        4, DATEADD(MONTH, -1, GETDATE()), DATEADD(MONTH, 5, GETDATE()), 200000.00, 'En Desarrollo', 'Alta');

-- Proyecto 5: Southern Copper (Finalizado)
INSERT INTO Proyectos (Id, Nombre, Descripcion, ClienteId, FechaInicio, FechaFinEstimada, Presupuesto, Estado, Prioridad) 
VALUES (5, 'Dashboard Ejecutivo de Producción', 
        'Visualización en tiempo real de indicadores KPI de producción minera con dashboards interactivos en Power BI.', 
        5, DATEADD(MONTH, -6, GETDATE()), DATEADD(MONTH, -1, GETDATE()), 45000.00, 'Finalizado', 'Media');

SET IDENTITY_INSERT Proyectos OFF;

PRINT '✓ 5 proyectos insertados';
GO

-- ====================================================================================
-- PASO 5: INSERTAR TAREAS
-- ====================================================================================
PRINT 'Insertando tareas...';

-- Obtener IDs de usuarios
DECLARE @AdminId NVARCHAR(450);
DECLARE @LiderId NVARCHAR(450);
DECLARE @DesarrolladorId NVARCHAR(450);

SELECT @AdminId = Id FROM AspNetUsers WHERE Email = 'admin@techsolutions.com';
SELECT @LiderId = Id FROM AspNetUsers WHERE Email = 'lider@techsolutions.com';
SELECT @DesarrolladorId = Id FROM AspNetUsers WHERE Email = 'desarrollador@techsolutions.com';

SET IDENTITY_INSERT Tareas ON;

-- TAREAS DEL PROYECTO 1: Minera Yanacocha (En Desarrollo)
INSERT INTO Tareas (Id, Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
(1, 'Diseñar Arquitectura en Azure (VMs, VNet, Storage)', 1, 'Finalizado', 'Alta', @LiderId, DATEADD(DAY, -10, GETDATE())),
(2, 'Configurar VPN Site-to-Site entre Cajamarca y Azure', 1, 'Finalizado', 'Alta', @AdminId, DATEADD(DAY, -5, GETDATE())),
(3, 'Desarrollar API REST para Gestión de Activos', 1, 'En Progreso', 'Alta', @DesarrolladorId, DATEADD(DAY, 10, GETDATE())),
(4, 'Integrar sensores IoT con Azure IoT Hub', 1, 'Pendiente', 'Media', @DesarrolladorId, DATEADD(DAY, 20, GETDATE())),
(5, 'Crear dashboards de monitoreo en tiempo real', 1, 'Pendiente', 'Media', @LiderId, DATEADD(DAY, 30, GETDATE()));

-- TAREAS DEL PROYECTO 2: App Yape 3.0 (En Pruebas)
INSERT INTO Tareas (Id, Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
(6, 'Desarrollar módulo de pagos con QR dinámico', 2, 'Finalizado', 'Alta', @DesarrolladorId, DATEADD(DAY, -20, GETDATE())),
(7, 'Implementar transferencias internacionales (Swift)', 2, 'Finalizado', 'Alta', @AdminId, DATEADD(DAY, -15, GETDATE())),
(8, 'Pruebas de carga y performance (10,000 TPS)', 2, 'En Progreso', 'Alta', @LiderId, DATEADD(DAY, 5, GETDATE())),
(9, 'Corregir bugs en autenticación biométrica', 2, 'En Progreso', 'Alta', @DesarrolladorId, DATEADD(DAY, 3, GETDATE())),
(10, 'Realizar pruebas de penetración (PenTest)', 2, 'Pendiente', 'Alta', @AdminId, DATEADD(DAY, 8, GETDATE()));

-- TAREAS DEL PROYECTO 3: Intranet RRHH (Planificación)
INSERT INTO Tareas (Id, Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
(11, 'Levantar requerimientos funcionales con RRHH', 3, 'En Progreso', 'Alta', @LiderId, DATEADD(DAY, 7, GETDATE())),
(12, 'Diseñar mockups de interfaz de usuario', 3, 'Pendiente', 'Media', @DesarrolladorId, DATEADD(DAY, 15, GETDATE())),
(13, 'Definir arquitectura de base de datos', 3, 'Pendiente', 'Media', @AdminId, DATEADD(DAY, 20, GETDATE()));

-- TAREAS DEL PROYECTO 4: Sistema Omnicanal Telefónica (En Desarrollo)
INSERT INTO Tareas (Id, Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
(14, 'Desarrollar API de integración con CRM Salesforce', 4, 'En Progreso', 'Alta', @DesarrolladorId, DATEADD(DAY, 12, GETDATE())),
(15, 'Implementar chatbot con IA (OpenAI GPT)', 4, 'En Progreso', 'Alta', @AdminId, DATEADD(DAY, 18, GETDATE())),
(16, 'Configurar enrutamiento inteligente de tickets', 4, 'Pendiente', 'Media', @LiderId, DATEADD(DAY, 25, GETDATE())),
(17, 'Desarrollar módulo de reportes analíticos', 4, 'Pendiente', 'Baja', @DesarrolladorId, DATEADD(DAY, 35, GETDATE()));

-- TAREAS DEL PROYECTO 5: Dashboard Southern Copper (Finalizado)
INSERT INTO Tareas (Id, Descripcion, ProyectoId, Estado, Prioridad, ResponsableId, FechaLimite) VALUES 
(18, 'Conectar Power BI con base de datos Oracle', 5, 'Finalizado', 'Alta', @AdminId, DATEADD(DAY, -30, GETDATE())),
(19, 'Crear visualizaciones de KPIs de producción', 5, 'Finalizado', 'Alta', @LiderId, DATEADD(DAY, -20, GETDATE())),
(20, 'Capacitar al personal en uso de dashboards', 5, 'Finalizado', 'Media', @DesarrolladorId, DATEADD(DAY, -10, GETDATE()));

SET IDENTITY_INSERT Tareas OFF;

PRINT '✓ 20 tareas insertadas';
GO

-- ====================================================================================
-- PASO 6: INSERTAR REPORTES HISTÓRICOS
-- ====================================================================================
PRINT 'Insertando reportes históricos...';

SET IDENTITY_INSERT Reportes ON;

INSERT INTO Reportes (Id, Titulo, TipoFormato, FechaGeneracion, GeneradoPor) VALUES 
(1, 'Reporte Mensual de Proyectos - Enero 2026', 'PDF', DATEADD(DAY, -15, GETDATE()), 'Juan Pérez - Líder de Proyecto'),
(2, 'Estado de Tareas por Desarrollador', 'Excel', DATEADD(DAY, -10, GETDATE()), 'Administrador del Sistema'),
(3, 'Análisis de Presupuestos Q1 2026', 'PDF', DATEADD(DAY, -5, GETDATE()), 'Juan Pérez - Líder de Proyecto'),
(4, 'Reporte de Productividad Semanal', 'Excel', DATEADD(DAY, -2, GETDATE()), 'María García - Desarrolladora');

SET IDENTITY_INSERT Reportes OFF;

PRINT '✓ 4 reportes históricos insertados';
GO

-- ====================================================================================
-- PASO 7: VERIFICACIÓN DE DATOS
-- ====================================================================================
PRINT '';
PRINT '==============================================';
PRINT 'RESUMEN DE DATOS INSERTADOS:';
PRINT '=================================='
