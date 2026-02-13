using TechSolutions_program.Services.Interfaces;
using TechSolutions_program.Services.Strategies;

namespace TechSolutions_program.Services.Implementations
{
    public class ReporteService : IReporteService
    {
        private readonly IEnumerable<IReporteStrategy> _strategies;

        public ReporteService(IEnumerable<IReporteStrategy> strategies)
        {
            _strategies = strategies;
        }

        public Task<ReporteResultado> GenerarAsync(string tipoReporte, int proyectoId)
        {
            if (string.IsNullOrWhiteSpace(tipoReporte))
            {
                throw new ArgumentException("Tipo de reporte no válido.", nameof(tipoReporte));
            }

            var strategy = _strategies.FirstOrDefault(s =>
                string.Equals(s.Tipo, tipoReporte, StringComparison.OrdinalIgnoreCase));

            if (strategy == null)
            {
                throw new InvalidOperationException("No existe una estrategia para el tipo solicitado.");
            }

            return strategy.Generar(proyectoId);
        }
    }
}
