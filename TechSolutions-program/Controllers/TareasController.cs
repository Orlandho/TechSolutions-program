using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechSolutions_program.Models;
using TechSolutions_program.Services.Interfaces;

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

        /// <summary>
        /// GET: /Tareas/Index
        /// Lista todas las tareas del sistema (solo para Líderes)
        /// Usado en: <a asp-controller="Tareas" asp-action="Index">Gestionar Tareas</a>
        /// </summary>
        [Authorize(Roles = "Lider")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var todas = await _tareaService.GetTareasAsync();
                return View(todas);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(Array.Empty<Tarea>());
            }
        }

        /// <summary>
        /// GET: /Tareas/MisTareas
        /// Muestra solo las tareas asignadas al desarrollador actual
        /// Usado en: <a asp-action="MisTareas">Mis Tareas</a>
        /// </summary>
        [Authorize(Roles = "Desarrollador")]
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

        /// <summary>
        /// GET: /Tareas/Create
        /// Muestra el formulario para crear una nueva tarea (solo para Líderes)
        /// Usado en: <a asp-action="Create">Nueva Tarea</a>
        /// </summary>
        [Authorize(Roles = "Lider")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Task.CompletedTask;
            return View();
        }

        /// <summary>
        /// POST: /Tareas/Create
        /// Procesa el formulario de creación de tarea (solo para Líderes)
        /// Usado en: <form asp-action="Create"> con botón submit
        /// </summary>
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

        /// <summary>
        /// GET: /Tareas/Edit/5
        /// Muestra el formulario de edición de una tarea (solo para Líderes)
        /// Usado en: <a asp-action="Edit" asp-route-id="@tarea.Id">Editar</a>
        /// </summary>
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

        /// <summary>
        /// POST: /Tareas/Edit/5
        /// Procesa el formulario de edición de tarea (solo para Líderes)
        /// Usado en: <form asp-action="Edit"> con botón "Guardar Cambios"
        /// </summary>
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

        /// <summary>
        /// GET: /Tareas/Delete/5
        /// Muestra la confirmación para eliminar una tarea (solo para Líderes)
        /// Usado en: <a asp-action="Delete" asp-route-id="@tarea.Id">Eliminar</a>
        /// </summary>
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

        /// <summary>
        /// POST: /Tareas/Delete/5
        /// Elimina permanentemente una tarea (solo para Líderes)
        /// Usado en: <form asp-action="Delete"> con botón "Confirmar Eliminación"
        /// </summary>
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

        /// <summary>
        /// POST: /Tareas/CambiarEstado
        /// Cambia el estado de una tarea (Pendiente, En Progreso, Finalizado)
        /// Los Desarrolladores pueden cambiar el estado de sus tareas asignadas
        /// Usado en: <form asp-action="CambiarEstado" asp-route-id="@tarea.Id">
        ///           <button type="submit" name="nuevoEstado" value="En Progreso">
        /// </summary>
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
