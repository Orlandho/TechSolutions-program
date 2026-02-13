using TechSolutions_program.Models;

namespace TechSolutions_program.Services.Interfaces
{
    public interface IProyectoService
    {
        Task<IEnumerable<Proyecto>> GetProyectosAsync();
        Task<Proyecto?> GetByIdAsync(int id);
        Task CrearAsync(Proyecto proyecto);
        Task ActualizarAsync(Proyecto proyecto);
        Task EliminarAsync(int id);
    }
}
