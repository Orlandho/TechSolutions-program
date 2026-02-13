using TechSolutions_program.Services.Interfaces;
using TechSolutions_program.Services.Strategies;

namespace TechSolutions_program.Services.Implementations
{
    /// <summary>
    /// Servicio para la generación de reportes utilizando el patrón Strategy
    /// </summary>
    public class ReporteService : IReporteService
    {
        private readonly IEnumerable<IReporteStrategy> _strategies;

        public ReporteService(IEnumerable<IReporteStrategy> strategies)
        {
            _strategies = strategies;
        }

        /// <summary>
        /// Genera un reporte del tipo especificado para un proyecto
        /// Este método es llamado desde ReportesController para generar PDF o Excel
        /// </summary>
        /// <param name="tipoReporte">Tipo de reporte: "pdf" o "excel"</param>
        /// <param name="proyectoId">ID del proyecto para el que se genera el reporte</param>
        /// <returns>ReporteResultado con los datos binarios del archivo generado</returns>
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
