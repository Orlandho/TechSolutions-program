using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TechSolutions_program.Models;
using TechSolutions_program.Services.Interfaces;

namespace TechSolutions_program.Controllers
{
    /*
     * ====================================================================================
     * CAPA DE PRESENTACIÓN: CONTROLADOR DE SEGUIMIENTO Y MONITOREO
     * ====================================================================================
     * REFERENCIA DOCUMENTAL: 
     * - CU-05: Visualizar Dashboard (Indicadores de progreso y semáforos).
     * - Objetivo General: Mejorar la visibilidad del progreso y cumplimiento.
     * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
     * Gestiona la visualización de alto nivel del estado de los proyectos. Es el 
     * nexo entre los datos operativos de las tareas y la visión estratégica del Líder.
     * * FUNCIONALIDADES BASADAS EN EL DISEÑO:
     * 1. Visualización de Indicadores: Procesa la lógica para mostrar "Semáforos de 
     * Cumplimiento" (Verde, Amarillo, Rojo) basados en la comparación entre las 
     * fechas estimadas y el progreso real de las tareas.
     * 2. Trazabilidad: Expone los datos de avance histórico, permitiendo identificar
     * cuellos de botella en el desarrollo antes de que afecten el presupuesto final,
     * atacando directamente el problema de falta de visibilidad del caso integrador.
     * ====================================================================================
     */
    public class SeguimientoController : Controller
    {
        private readonly IProyectoService _proyectoService;
        private readonly ITareaService _tareaService;

        public SeguimientoController(IProyectoService proyectoService, ITareaService tareaService)
        {
            _proyectoService = proyectoService;
            _tareaService = tareaService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var proyectos = await _proyectoService.GetProyectosAsync();
                var tareas = await _tareaService.GetTareasAsync();

                var model = new DashboardViewModel
                {
                    TotalProyectos = proyectos.Count(),
                    PresupuestoTotal = proyectos.Sum(p => p.Presupuesto),
                    TareasPendientes = tareas.Count(t => t.Estado == "Pendiente"),
                    TareasCompletadas = tareas.Count(t => t.Estado == "Terminado" || t.Estado == "Finalizado")
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new DashboardViewModel());
            }
        }
    }
}
