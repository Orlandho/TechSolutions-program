using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TechSolutions_program.Data;

namespace TechSolutions_program.Services.Strategies
{
    public class PdfReporteStrategy : IReporteStrategy
    {
        private readonly ApplicationDbContext _context;

        public PdfReporteStrategy(ApplicationDbContext context)
        {
            _context = context;
            // Configurar licencia de QuestPDF para uso comunitario
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public string Tipo => "PDF";

        public async Task<ReporteResultado> Generar(int proyectoId)
        {
            // Obtener datos del proyecto con sus relaciones
            var proyecto = await _context.Proyectos
                .Include(p => p.Cliente)
                .Include(p => p.Tareas)
                .FirstOrDefaultAsync(p => p.Id == proyectoId);

            if (proyecto == null)
            {
                throw new InvalidOperationException($"No se encontró el proyecto con ID {proyectoId}");
            }

            // Generar el PDF
            var contenido = new MemoryStream();
            
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    // Encabezado
                    page.Header()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("TechSolutions").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                            text.AlignCenter();
                        });

                    // Contenido del reporte
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            // Título del reporte
                            col.Item().AlignCenter().Text($"Reporte del Proyecto #{proyectoId}")
                                .FontSize(18).Bold().FontColor(Colors.Grey.Darken3);

                            col.Item().PaddingTop(0.5f, Unit.Centimetre);

                            // Información del proyecto
                            col.Item().Text("Información General").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().PaddingBottom(0.3f, Unit.Centimetre).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            col.Item().PaddingTop(0.3f, Unit.Centimetre).Row(row =>
                            {
                                row.RelativeItem().Column(column =>
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Nombre: ").Bold();
                                        text.Span(proyecto.Nombre);
                                    });

                                    column.Item().PaddingTop(5).Text(text =>
                                    {
                                        text.Span("Cliente: ").Bold();
                                        text.Span(proyecto.Cliente?.RazonSocial ?? "N/A");
                                    });

                                    column.Item().PaddingTop(5).Text(text =>
                                    {
                                        text.Span("Estado: ").Bold();
                                        text.Span(proyecto.Estado).FontColor(
                                            proyecto.Estado == "Finalizado" ? Colors.Green.Darken2 :
                                            proyecto.Estado == "En Desarrollo" ? Colors.Orange.Darken1 :
                                            Colors.Grey.Darken1);
                                    });

                                    column.Item().PaddingTop(5).Text(text =>
                                    {
                                        text.Span("Prioridad: ").Bold();
                                        text.Span(proyecto.Prioridad).FontColor(
                                            proyecto.Prioridad == "Alta" ? Colors.Red.Darken1 :
                                            proyecto.Prioridad == "Media" ? Colors.Orange.Medium :
                                            Colors.Blue.Lighten2);
                                    });
                                });

                                row.RelativeItem().Column(column =>
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Presupuesto: ").Bold();
                                        text.Span(proyecto.Presupuesto.ToString("C2"));
                                    });

                                    column.Item().PaddingTop(5).Text(text =>
                                    {
                                        text.Span("Fecha Inicio: ").Bold();
                                        text.Span(proyecto.FechaInicio.ToString("dd/MM/yyyy"));
                                    });

                                    column.Item().PaddingTop(5).Text(text =>
                                    {
                                        text.Span("Fecha Fin Estimada: ").Bold();
                                        text.Span(proyecto.FechaFinEstimada.ToString("dd/MM/yyyy"));
                                    });

                                    column.Item().PaddingTop(5).Text(text =>
                                    {
                                        text.Span("Duración: ").Bold();
                                        var dias = (proyecto.FechaFinEstimada - proyecto.FechaInicio).Days;
                                        text.Span($"{dias} días");
                                    });
                                });
                            });

                            // Descripción
                            if (!string.IsNullOrEmpty(proyecto.Descripcion))
                            {
                                col.Item().PaddingTop(0.5f, Unit.Centimetre);
                                col.Item().Text("Descripción").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                                col.Item().PaddingBottom(0.3f, Unit.Centimetre).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                                col.Item().PaddingTop(0.3f, Unit.Centimetre).Text(proyecto.Descripcion).FontSize(10);
                            }

                            // Tareas
                            col.Item().PaddingTop(0.8f, Unit.Centimetre);
                            col.Item().Text("Tareas del Proyecto").FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                            col.Item().PaddingBottom(0.3f, Unit.Centimetre).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            if (proyecto.Tareas != null && proyecto.Tareas.Any())
                            {
                                col.Item().PaddingTop(0.3f, Unit.Centimetre).Table(table =>
                                {
                                    // Definir columnas
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);  // Descripción
                                        columns.RelativeColumn(1);  // Estado
                                        columns.RelativeColumn(1);  // Prioridad
                                        columns.RelativeColumn(1);  // Fecha Límite
                                    });

                                    // Encabezado de tabla
                                    table.Header(header =>
                                    {
                                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Descripción").Bold();
                                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Estado").Bold();
                                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Prioridad").Bold();
                                        header.Cell().Background(Colors.Blue.Lighten3).Padding(5).Text("Fecha Límite").Bold();
                                    });

                                    // Filas de tareas
                                    foreach (var tarea in proyecto.Tareas.OrderBy(t => t.FechaLimite))
                                    {
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text(tarea.Descripcion).FontSize(9);
                                        
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text(tarea.Estado).FontSize(9);
                                        
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text(tarea.Prioridad).FontSize(9);
                                        
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text(tarea.FechaLimite?.ToString("dd/MM/yyyy") ?? "N/A").FontSize(9);
                                    }
                                });

                                // Resumen de tareas
                                col.Item().PaddingTop(0.5f, Unit.Centimetre).Row(row =>
                                {
                                    var totalTareas = proyecto.Tareas.Count;
                                    var tareasFinalizadas = proyecto.Tareas.Count(t => t.Estado == "Finalizado");
                                    var tareasEnProgreso = proyecto.Tareas.Count(t => t.Estado == "En Progreso");
                                    var tareasPendientes = proyecto.Tareas.Count(t => t.Estado == "Pendiente");

                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("Total de tareas: ").Bold().FontSize(10);
                                        text.Span($"{totalTareas}").FontSize(10);
                                    });

                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("Finalizadas: ").Bold().FontSize(10);
                                        text.Span($"{tareasFinalizadas}").FontSize(10).FontColor(Colors.Green.Darken2);
                                    });

                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("En Progreso: ").Bold().FontSize(10);
                                        text.Span($"{tareasEnProgreso}").FontSize(10).FontColor(Colors.Orange.Darken1);
                                    });

                                    row.RelativeItem().Text(text =>
                                    {
                                        text.Span("Pendientes: ").Bold().FontSize(10);
                                        text.Span($"{tareasPendientes}").FontSize(10).FontColor(Colors.Grey.Darken1);
                                    });
                                });
                            }
                            else
                            {
                                col.Item().PaddingTop(0.3f, Unit.Centimetre)
                                    .Text("No hay tareas asignadas a este proyecto.")
                                    .FontSize(10).Italic().FontColor(Colors.Grey.Medium);
                            }
                        });

                    // Pie de página
                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Generado el: ").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(9).Bold();
                            text.Span(" | TechSolutions - Sistema de Gestión de Proyectos").FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                });
            }).GeneratePdf(contenido);

            contenido.Position = 0;
            return new ReporteResultado(contenido, "application/pdf", $"Proyecto_{proyectoId}_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}
