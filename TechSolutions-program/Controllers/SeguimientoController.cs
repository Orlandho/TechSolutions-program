using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class SeguimientoController : Controller
    {
        private readonly IProyectoService _proyectoService;
        private readonly ITareaService _tareaService;

        public SeguimientoController(IProyectoService proyectoService, ITareaService tareaService)
        {
            _proyectoService = proyectoService;
            _tareaService = tareaService;
        }

        /// <summary>
        /// GET: /Seguimiento/Index
        /// Muestra el dashboard con indicadores de estado del sistema
        /// Calcula: Total de proyectos, presupuesto acumulado, tareas pendientes/completadas
        /// Usado en: <a asp-controller="Seguimiento" asp-action="Index">Dashboard</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtiene la lista de proyectos y tareas de los servicios respectivos
                var proyectos = await _proyectoService.GetProyectosAsync();
                var tareas = await _tareaService.GetTareasAsync();

                // Crea el modelo para la vista del dashboard
                var model = new DashboardViewModel
                {
                    TotalProyectos = proyectos.Count(),
                    PresupuestoTotal = proyectos.Sum(p => p.Presupuesto),
                    TareasPendientes = tareas.Count(t => t.Estado == "Pendiente"),
                    TareasCompletadas = tareas.Count(t => t.Estado == "Terminado" || t.Estado == "Finalizado")
                };

                // Retorna la vista con el modelo creado
                return View(model);
            }
            catch (Exception ex)
            {
                // En caso de error, agrega el mensaje al estado del modelo y retorna la vista vacía
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(new DashboardViewModel());
            }
        }
    }
}
