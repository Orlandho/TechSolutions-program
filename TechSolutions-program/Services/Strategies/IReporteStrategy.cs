namespace TechSolutions_program.Services.Strategies
{
    public interface IReporteStrategy
    {
        string Tipo { get; }
        Task<ReporteResultado> Generar(int proyectoId);
    }
}
