using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSolutions_program.Models;
using TechSolutions_program.Services.Interfaces;

namespace TechSolutions_program.Controllers
{
    /*
     * ====================================================================================
     * CAPA DE PRESENTACIÓN: CONTROLADOR DE GESTIÓN DE PROYECTOS
     * ====================================================================================
     * REFERENCIA DOCUMENTAL: 
     * - CU-01: Registrar Proyecto (Alta de contratos).
     * - CU-05: Visualizar Dashboard (Listado maestro y estado).
     * - Sección 5: Seguridad (Segregación de funciones y RBAC).
     * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
     * Componente orquestador que gestiona el ciclo de vida administrativo de los proyectos.
     * Actúa como la barrera de entrada para la manipulación de datos sensibles (Presupuestos, Fechas).
     * * CUMPLIMIENTO DE ARQUITECTURA (3 CAPAS):
     * 1. Desacoplamiento: Este controlador NO accede a la base de datos directamente. 
     * Delega estrictamente toda la lógica de validación y cálculo al 'ProyectoService',
     * cumpliendo el principio de responsabilidad única.
     * 2. Seguridad RBAC (Roles): Implementa la restricción crítica donde solo el usuario 
     * con el Claim de Rol 'Lider' o 'Administrador' puede ejecutar acciones de escritura 
     * (Create, Edit, Delete). Los 'Desarrolladores' tienen acceso restringido (Solo lectura).
     * 3. Integridad de Datos: Garantiza que no se creen proyectos con presupuestos negativos
     * o fechas incoherentes antes de enviarlos a la capa de negocio.
     * ====================================================================================
     */
    [Authorize]
    public class ProyectosController : Controller
    {
        private readonly IProyectoService _proyectoService;

        public ProyectosController(IProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
        }

        /// <summary>
        /// GET: /Proyectos/Index
        /// Lista todos los proyectos disponibles
        /// Usado en: <a asp-controller="Proyectos" asp-action="Index">Ver Proyectos</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var proyectos = await _proyectoService.GetProyectosAsync();
                return View(proyectos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los proyectos: {ex.Message}";
                return View(Array.Empty<Proyecto>());
            }
        }

        /// <summary>
        /// GET: /Proyectos/Details/5
        /// Muestra los detalles completos de un proyecto específico
        /// Usado en: <a asp-action="Details" asp-route-id="@proyecto.Id">Ver Detalles</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                if (proyecto == null)
                {
                    TempData["ErrorMessage"] = "El proyecto solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                return View(proyecto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los detalles del proyecto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// GET: /Proyectos/Create
        /// Muestra el formulario para crear un nuevo proyecto
        /// Solo accesible para roles: Lider, Administrador
        /// Usado en: <a asp-action="Create">Nuevo Proyecto</a>
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                // Cargar lista de clientes para el dropdown
                var clientes = await _proyectoService.GetClientesAsync();
                ViewBag.Clientes = clientes;
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el formulario: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /Proyectos/Create
        /// Procesa el formulario de creación de proyecto
        /// Solo accesible para roles: Lider, Administrador
        /// Usado en: <form asp-action="Create"> con botón submit
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                // Recargar clientes en caso de error de validación
                var clientes = await _proyectoService.GetClientesAsync();
                ViewBag.Clientes = clientes;
                TempData["ErrorMessage"] = "Por favor, corrija los errores en el formulario.";
                return View(proyecto);
            }

            try
            {
                await _proyectoService.CrearAsync(proyecto);
                TempData["SuccessMessage"] = $"El proyecto '{proyecto.Nombre}' se creó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Recargar clientes en caso de error
                var clientes = await _proyectoService.GetClientesAsync();
                ViewBag.Clientes = clientes;
                TempData["ErrorMessage"] = $"Error al crear el proyecto: {ex.Message}";
                return View(proyecto);
            }
        }

        /// <summary>
        /// GET: /Proyectos/Edit/5
        /// Muestra el formulario de edición de un proyecto existente
        /// Solo accesible para roles: Lider, Administrador
        /// Usado en: <a asp-action="Edit" asp-route-id="@proyecto.Id">Editar</a>
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                if (proyecto == null)
                {
                    TempData["ErrorMessage"] = "El proyecto solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                // Cargar clientes para el dropdown
                var clientes = await _proyectoService.GetClientesAsync();
                ViewBag.Clientes = clientes;

                return View(proyecto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el proyecto para editar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /Proyectos/Edit/5
        /// Procesa el formulario de edición de proyecto
        /// Solo accesible para roles: Lider, Administrador
        /// Usado en: <form asp-action="Edit"> con botón "Guardar Cambios"
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proyecto proyecto)
        {
            if (id != proyecto.Id)
            {
                TempData["ErrorMessage"] = "Error de validación: ID de proyecto no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                // Recargar clientes en caso de error
                var clientes = await _proyectoService.GetClientesAsync();
                ViewBag.Clientes = clientes;
                TempData["ErrorMessage"] = "Por favor, corrija los errores en el formulario.";
                return View(proyecto);
            }

            try
            {
                await _proyectoService.ActualizarAsync(proyecto);
                TempData["SuccessMessage"] = $"El proyecto '{proyecto.Nombre}' se actualizó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Recargar clientes en caso de error
                var clientes = await _proyectoService.GetClientesAsync();
                ViewBag.Clientes = clientes;
                TempData["ErrorMessage"] = $"Error al actualizar el proyecto: {ex.Message}";
                return View(proyecto);
            }
        }

        /// <summary>
        /// GET: /Proyectos/Delete/5
        /// Muestra la página de confirmación para eliminar un proyecto
        /// Solo accesible para roles: Lider, Administrador
        /// Usado en: <a asp-action="Delete" asp-route-id="@proyecto.Id">Eliminar</a>
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                if (proyecto == null)
                {
                    TempData["ErrorMessage"] = "El proyecto solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                return View(proyecto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el proyecto para eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /Proyectos/Delete/5
        /// Elimina permanentemente un proyecto de la base de datos
        /// Solo accesible para roles: Lider, Administrador
        /// Usado en: <form asp-action="Delete"> con botón "Confirmar Eliminación"
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                var nombreProyecto = proyecto?.Nombre ?? $"ID {id}";
                
                await _proyectoService.EliminarAsync(id);
                TempData["SuccessMessage"] = $"El proyecto '{nombreProyecto}' se eliminó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el proyecto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
