using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSolutions_program.Models;
using TechSolutions_program.Services;

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
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(Array.Empty<Proyecto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                if (proyecto == null)
                {
                    return NotFound();
                }

                return View(proyecto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Task.CompletedTask;
            return View();
        }

        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                return View(proyecto);
            }

            try
            {
                await _proyectoService.CrearAsync(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(proyecto);
            }
        }

        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                if (proyecto == null)
                {
                    return NotFound();
                }

                return View(proyecto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proyecto proyecto)
        {
            if (id != proyecto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(proyecto);
            }

            try
            {
                await _proyectoService.ActualizarAsync(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(proyecto);
            }
        }

        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var proyecto = await _proyectoService.GetByIdAsync(id);
                if (proyecto == null)
                {
                    return NotFound();
                }

                return View(proyecto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _proyectoService.EliminarAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var proyecto = await _proyectoService.GetByIdAsync(id);
                return View(proyecto);
            }
        }
    }
}
