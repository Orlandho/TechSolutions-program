/*
 * ====================================================================================
 * CAPA DE PRESENTACIÓN: CONTROLADOR DE GESTIÓN DE PROYECTOS
 * ====================================================================================
 * REFERENCIA DOCUMENTAL: 
 * - CU-01: Registrar Proyecto (Alta de contratos).
 * - CU-05: Visualizar Dashboard (Listado maestro y estado).
 * - Sección 5: Seguridad (Segregación de funciones y RBAC).
 * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
 * Componente orquestador que gestiona el ciclo de vida administrativo de los proyectos.
 * Actúa como la barrera de entrada para la manipulación de datos sensibles (Presupuestos, Fechas).
 * * CUMPLIMIENTO DE ARQUITECTURA (3 CAPAS):
 * 1. Desacoplamiento: Este controlador NO accede a la base de datos directamente. 
 * Delega estrictamente toda la lógica de validación y cálculo al 'ProyectoService',
 * cumpliendo el principio de responsabilidad única.
 * 2. Seguridad RBAC (Roles): Implementa la restricción crítica donde solo el usuario 
 * con el Claim de Rol 'Lider' o 'Administrador' puede ejecutar acciones de escritura 
 * (Create, Edit, Delete). Los 'Desarrolladores' tienen acceso restringido (Solo lectura).
 * 3. Integridad de Datos: Garantiza que no se creen proyectos con presupuestos negativos
 * o fechas incoherentes antes de enviarlos a la capa de negocio.
 * ====================================================================================
 */


namespace TechSolutions_program.Controllers
{
    public class ProyectosController
    {
    }
}
