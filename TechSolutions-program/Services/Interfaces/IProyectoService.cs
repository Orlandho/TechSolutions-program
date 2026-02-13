using TechSolutions_program.Models;

namespace TechSolutions_program.Services.Interfaces
{
    /// <summary>
    /// Contrato de servicios para la lógica de negocio de Proyectos
    /// Implementado por: ProyectoService
    /// Consumido por: ProyectosController y SeguimientoController
    /// </summary>
    public interface IProyectoService
    {
        /// <summary>
        /// Obtiene todos los proyectos del sistema
        /// Usado en: ProyectosController.Index() y SeguimientoController.Index()
        /// </summary>
        Task<IEnumerable<Proyecto>> GetProyectosAsync();

        /// <summary>
        /// Obtiene un proyecto específico por su ID
        /// Usado en: ProyectosController.Details(), Edit(), Delete()
        /// </summary>
        Task<Proyecto?> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo proyecto en el sistema
        /// Usado en: ProyectosController.Create() [POST]
        /// </summary>
        Task CrearAsync(Proyecto proyecto);

        /// <summary>
        /// Actualiza la información de un proyecto existente
        /// Usado en: ProyectosController.Edit() [POST]
        /// </summary>
        Task ActualizarAsync(Proyecto proyecto);

        /// <summary>
        /// Elimina un proyecto del sistema
        /// Usado en: ProyectosController.DeleteConfirmed() [POST]
        /// </summary>
        Task EliminarAsync(int id);

        /// <summary>
        /// Obtiene todos los clientes para dropdowns
        /// Usado en: ProyectosController.Create(), Edit()
        /// </summary>
        Task<IEnumerable<Cliente>> GetClientesAsync();
    }
}
