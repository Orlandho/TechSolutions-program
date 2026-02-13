using TechSolutions_program.Models;

namespace TechSolutions_program.Services
{
    public class TareaService : ITareaService
    {
        /*
         * ====================================================================================
         * CAPA DE LÓGICA DE NEGOCIO: GESTOR DE TAREAS
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Modelo de Análisis: Clase de Control "GestorTareas".
         * - CU-02: Gestionar Tareas y CU-03: Actualizar Avance.
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Centraliza las reglas de negocio para la operatividad diaria de los desarrolladores.
         * * FUNCIONES CLAVE:
         * 1. Asignación: Verifica que la tarea se asigne a un usuario existente y con rol 'Desarrollador'.
         * 2. Control de Estado: Gestiona la transición de estados (Pendiente -> En Progreso -> Finalizado).
         * 3. Cálculo de Impacto: Al cerrar una tarea, dispara el recálculo del avance general 
         * del proyecto asociado.
         * ====================================================================================
         */

        public Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            return Task.FromResult<IEnumerable<Tarea>>(Array.Empty<Tarea>());
        }

        public Task<IEnumerable<Tarea>> GetTareasPorResponsableAsync(string responsableId)
        {
            return Task.FromResult<IEnumerable<Tarea>>(Array.Empty<Tarea>());
        }

        public Task<Tarea?> GetByIdAsync(int id)
        {
            return Task.FromResult<Tarea?>(null);
        }

        public Task CrearAsync(Tarea tarea)
        {
            return Task.CompletedTask;
        }

        public Task ActualizarAsync(Tarea tarea)
        {
            return Task.CompletedTask;
        }

        public Task EliminarAsync(int id)
        {
            return Task.CompletedTask;
        }

        public Task CambiarEstadoAsync(int id, string nuevoEstado)
        {
            return Task.CompletedTask;
        }
    }
}
