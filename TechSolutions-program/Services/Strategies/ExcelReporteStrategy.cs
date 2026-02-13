namespace TechSolutions_program.Services.Strategies
{
    public class ExcelReporteStrategy : IReporteStrategy
    {
        public string Tipo => "Excel";

        public async Task<ReporteResultado> Generar(int proyectoId)
        {
            var contenido = new MemoryStream();
            await using (var writer = new StreamWriter(contenido, leaveOpen: true))
            {
                await writer.WriteAsync($"Reporte Excel del proyecto {proyectoId}");
            }

            contenido.Position = 0;
            return new ReporteResultado(
                contenido,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"reporte-{proyectoId}.xlsx");
        }
    }
}
