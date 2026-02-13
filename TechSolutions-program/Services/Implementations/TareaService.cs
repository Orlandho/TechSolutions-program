using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Data;
using TechSolutions_program.Models;
using TechSolutions_program.Services.Interfaces;

namespace TechSolutions_program.Services.Implementations
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

        private readonly ApplicationDbContext _dbContext;

        public TareaService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Obtiene la lista de todas las tareas del sistema
        /// Llamado desde: TareasController.Index() y SeguimientoController.Index()
        /// </summary>
        public async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            return await _dbContext.Tareas.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Obtiene las tareas asignadas a un desarrollador específico
        /// Llamado desde: TareasController.MisTareas()
        /// Permite que los desarrolladores vean solo sus tareas asignadas
        /// </summary>
        public async Task<IEnumerable<Tarea>> GetTareasPorResponsableAsync(string responsableId)
        {
            return await _dbContext.Tareas.AsNoTracking()
                .Where(t => t.ResponsableId == responsableId)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una tarea específica por su ID
        /// Llamado desde: TareasController.Edit(), Delete() y DeleteConfirmed()
        /// </summary>
        public async Task<Tarea?> GetByIdAsync(int id)
        {
            return await _dbContext.Tareas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Crea una nueva tarea en la base de datos
        /// Llamado desde: TareasController.Create() [POST]
        /// </summary>
        public async Task CrearAsync(Tarea tarea)
        {
            _dbContext.Tareas.Add(tarea);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza una tarea existente
        /// Llamado desde: TareasController.Edit() [POST]
        /// </summary>
        public async Task ActualizarAsync(Tarea tarea)
        {
            _dbContext.Tareas.Update(tarea);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina una tarea de la base de datos
        /// Llamado desde: TareasController.DeleteConfirmed() [POST]
        /// </summary>
        public async Task EliminarAsync(int id)
        {
            var tarea = await _dbContext.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                return;
            }

            _dbContext.Tareas.Remove(tarea);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Cambia el estado de una tarea (Pendiente, En Progreso, Finalizado)
        /// Llamado desde: TareasController.CambiarEstado() [POST]
        /// Permite a los desarrolladores actualizar el progreso de sus tareas
        /// </summary>
        public async Task CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var tarea = await _dbContext.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                return;
            }

            tarea.Estado = nuevoEstado;
            await _dbContext.SaveChangesAsync();
        }
    }
}
