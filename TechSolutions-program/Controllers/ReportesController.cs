using Microsoft.AspNetCore.Mvc;
using TechSolutions_program.Services.Interfaces;

namespace TechSolutions_program.Controllers
{
    /*
     * ====================================================================================
     * CAPA DE PRESENTACIÓN: CONTROLADOR DE REPORTES Y ANALÍTICA
     * ====================================================================================
     * REFERENCIA DOCUMENTAL: 
     * - Ítem 3: Patrones de Diseño (Patrón Strategy para exportación dinámica).
     * - Requisito: Generación de reportes en formatos Excel y PDF.
     * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
     * Orquestador de la salida de datos gerenciales. Su función principal es permitir
     * la toma de decisiones basada en datos reales de productividad y presupuesto.
     * * APLICACIÓN DE PATRONES:
     * 1. Patrón Strategy: Este controlador no contiene la lógica de exportación; en 
     * su lugar, delega la creación del archivo a una estrategia específica (Excel o PDF)
     * seleccionada en tiempo de ejecución. Esto permite que el sistema sea extensible
     * sin modificar el código base, respetando el principio Open/Closed.
     * 2. Eficiencia: Optimiza la descarga de documentos pesados mediante flujos de 
     * datos (Streams), asegurando la escalabilidad del portal de TechSolutions.
     * ====================================================================================
     */
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> Descargar(string tipoReporte, int proyectoId)
        {
            var resultado = await _reporteService.GenerarAsync(tipoReporte, proyectoId);
            return File(resultado.Contenido, resultado.ContentType, resultado.NombreArchivo);
        }
    }
}
