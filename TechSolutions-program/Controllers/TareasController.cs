namespace TechSolutions_program.Controllers
{
    public class TareasController
    {

        /*
         * ====================================================================================
         * CAPA DE PRESENTACIÓN: CONTROLADOR DE TAREAS
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - CU-02: Gestionar Tareas (Asignación y Planificación).
         * - CU-03: Actualizar Avance (Reporte de desarrolladores).
         * - Actores: Líder de Proyecto (Asigna) vs Desarrollador (Ejecuta).
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Gestiona el ciclo de vida operativo del desarrollo. Permite la interacción 
         * diaria de los desarrolladores para marcar su progreso.
         * * SEGURIDAD Y REGLAS DE NEGOCIO (RBAC):
         * 1. Permisos Diferenciados (Políticas de Seguridad):
         * - El 'Líder' tiene acceso total (Crear, Asignar, Eliminar tareas).
         * - El 'Desarrollador' tiene acceso restringido: Solo puede visualizar sus 
         * tareas asignadas y cambiar el estado (ej. de 'Pendiente' a 'Finalizado').
         * 2. Trazabilidad: Cada cambio de estado queda registrado para alimentar el 
         * Dashboard de control en tiempo real.
         * ====================================================================================
         */

    }
}
