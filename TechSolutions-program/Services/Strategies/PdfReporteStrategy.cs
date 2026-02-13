namespace TechSolutions_program.Services.Strategies
{
    public class PdfReporteStrategy : IReporteStrategy
    {
        public string Tipo => "PDF";

        public async Task<ReporteResultado> Generar(int proyectoId)
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
