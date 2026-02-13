using TechSolutions_program.Models;

namespace TechSolutions_program.Services
{
    public interface ITareaService
    {
        Task<IEnumerable<Tarea>> GetTareasAsync();
        Task<IEnumerable<Tarea>> GetTareasPorResponsableAsync(string responsableId);
        Task<Tarea?> GetByIdAsync(int id);
        Task CrearAsync(Tarea tarea);
        Task ActualizarAsync(Tarea tarea);
        Task EliminarAsync(int id);
        Task CambiarEstadoAsync(int id, string nuevoEstado);
    }
}
