using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechSolutions_program.Models;
using TechSolutions_program.Services;

namespace TechSolutions_program.Controllers
{
    /*
     * ====================================================================================
     * CAPA DE PRESENTACIÓN: CONTROLADOR DE TAREAS
     * ====================================================================================
     * REFERENCIA DOCUMENTAL: 
     * - CU-02: Gestionar Tareas (Asignación y Planificación).
     * - CU-03: Actualizar Avance (Reporte de desarrolladores).
     * - Actores: Líder de Proyecto (Asigna) vs Desarrollador (Ejecuta).
     * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
     * Gestiona el ciclo de vida operativo del desarrollo. Permite la interacción 
     * diaria de los desarrolladores para marcar su progreso.
     * * SEGURIDAD Y REGLAS DE NEGOCIO (RBAC):
     * 1. Permisos Diferenciados (Políticas de Seguridad):
     * - El 'Líder' tiene acceso total (Crear, Asignar, Eliminar tareas).
     * - El 'Desarrollador' tiene acceso restringido: Solo puede visualizar sus 
     * tareas asignadas y cambiar el estado (ej. de 'Pendiente' a 'Finalizado').
     * 2. Trazabilidad: Cada cambio de estado queda registrado para alimentar el 
     * Dashboard de control en tiempo real.
     * ====================================================================================
     */
    [Authorize]
    public class TareasController : Controller
    {
        private readonly ITareaService _tareaService;
        private readonly UserManager<Usuario> _userManager;

        public TareasController(ITareaService tareaService, UserManager<Usuario> userManager)
        {
            _tareaService = tareaService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                if (User.IsInRole("Lider"))
                {
                    var todas = await _tareaService.GetTareasAsync();
                    return View(todas);
                }

                var usuario = await _userManager.GetUserAsync(User);
                if (usuario == null)
                {
                    return Challenge();
                }

                var asignadas = await _tareaService.GetTareasPorResponsableAsync(usuario.Id);
                return View(asignadas);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(Array.Empty<Tarea>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> MisTareas()
        {
            try
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (usuario == null)
                {
                    return Challenge();
                }

                var tareas = await _tareaService.GetTareasPorResponsableAsync(usuario.Id);
                return View(tareas);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(Array.Empty<Tarea>());
            }
        }

        [Authorize(Roles = "Lider")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Task.CompletedTask;
            return View();
        }

        [Authorize(Roles = "Lider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                return View(tarea);
            }

            try
            {
                await _tareaService.CrearAsync(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(tarea);
            }
        }

        [Authorize(Roles = "Lider")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var tarea = await _tareaService.GetByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound();
                }

                return View(tarea);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Lider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(tarea);
            }

            try
            {
                await _tareaService.ActualizarAsync(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(tarea);
            }
        }

        [Authorize(Roles = "Lider")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tarea = await _tareaService.GetByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound();
                }

                return View(tarea);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Lider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tareaService.EliminarAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var tarea = await _tareaService.GetByIdAsync(id);
                return View(tarea);
            }
        }

        [Authorize(Roles = "Desarrollador,Lider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                ModelState.AddModelError(string.Empty, "Estado no válido.");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _tareaService.CambiarEstadoAsync(id, nuevoEstado);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
