using TechSolutions_program.Models;

namespace TechSolutions_program.Services.Interfaces
{
    /// <summary>
    /// Contrato de servicios para la lógica de negocio de Tareas
    /// Implementado por: TareaService
    /// Consumido por: TareasController y SeguimientoController
    /// </summary>
    public interface ITareaService
    {
        /// <summary>
        /// Obtiene todas las tareas del sistema
        /// Usado en: TareasController.Index() y SeguimientoController.Index()
        /// </summary>
        Task<IEnumerable<Tarea>> GetTareasAsync();

        /// <summary>
        /// Obtiene las tareas asignadas a un desarrollador específico
        /// Usado en: TareasController.MisTareas() para la vista del desarrollador
        /// </summary>
        Task<IEnumerable<Tarea>> GetTareasPorResponsableAsync(string responsableId);

        /// <summary>
        /// Obtiene una tarea específica por su ID
        /// Usado en: TareasController.Edit(), Delete()
        /// </summary>
        Task<Tarea?> GetByIdAsync(int id);

        /// <summary>
        /// Crea una nueva tarea en el sistema
        /// Usado en: TareasController.Create() [POST]
        /// </summary>
        Task CrearAsync(Tarea tarea);

        /// <summary>
        /// Actualiza la información de una tarea existente
        /// Usado en: TareasController.Edit() [POST]
        /// </summary>
        Task ActualizarAsync(Tarea tarea);

        /// <summary>
        /// Elimina una tarea del sistema
        /// Usado in: TareasController.DeleteConfirmed() [POST]
        /// </summary>
        Task EliminarAsync(int id);

        /// <summary>
        /// Cambia el estado de una tarea (Pendiente, En Progreso, Finalizado)
        /// Usado en: TareasController.CambiarEstado() - permite a desarrolladores actualizar el avance
        /// Este método es clave para que los botones de cambio de estado funcionen en las vistas
        /// </summary>
        Task CambiarEstadoAsync(int id, string nuevoEstado);

        /// <summary>
        /// Obtiene todos los proyectos para dropdowns
        /// Usado en: TareasController.Create(), Edit()
        /// </summary>
        Task<IEnumerable<Proyecto>> GetProyectosAsync();

        /// <summary>
        /// Obtiene todos los usuarios (desarrolladores) para dropdowns
        /// Usado en: TareasController.Create(), Edit()
        /// </summary>
        Task<IEnumerable<Usuario>> GetUsuariosAsync();
    }
}
