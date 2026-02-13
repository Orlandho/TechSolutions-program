using TechSolutions_program.Services.Strategies;

namespace TechSolutions_program.Services.Interfaces
{
    public interface IReporteService
    {
        Task<ReporteResultado> GenerarAsync(string tipoReporte, int proyectoId);
    }
}
