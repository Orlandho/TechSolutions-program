namespace TechSolutions_program.Services.Strategies
{
    public class PdfReporteStrategy : IReporteStrategy
    {
        public async Task<ReporteResultado> GenerarAsync(int proyectoId)
        {
            var contenido = new MemoryStream();
            await using (var writer = new StreamWriter(contenido, leaveOpen: true))
            {
                await writer.WriteAsync($"Reporte PDF del proyecto {proyectoId}");
            }

            contenido.Position = 0;
            return new ReporteResultado(contenido, "application/pdf", $"reporte-{proyectoId}.pdf");
        }
    }
}
