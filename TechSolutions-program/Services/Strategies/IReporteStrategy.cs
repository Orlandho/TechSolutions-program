namespace TechSolutions_program.Services.Strategies
{
    public interface IReporteStrategy
    {
        Task<ReporteResultado> GenerarAsync(int proyectoId);
    }
}
