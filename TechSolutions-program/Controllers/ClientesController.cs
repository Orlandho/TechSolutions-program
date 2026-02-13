using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Data;
using TechSolutions_program.Models;

namespace TechSolutions_program.Controllers
{
    /// <summary>
    /// Controlador CRUD para la gestión de clientes de TechSolutions
    /// Requiere autenticación para acceder a cualquier acción
    /// </summary>
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ClientesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// GET: /Clientes/Index
        /// Muestra el listado de todos los clientes
        /// Usado en: <a asp-controller="Clientes" asp-action="Index">Ver Clientes</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var clientes = await _dbContext.Clientes.AsNoTracking().ToListAsync();
                return View(clientes);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los clientes: {ex.Message}";
                return View(new List<Cliente>());
            }
        }

        /// <summary>
        /// GET: /Clientes/Details/5
        /// Muestra los detalles de un cliente específico
        /// Usado en: <a asp-action="Details" asp-route-id="@cliente.Id">Ver Detalles</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cliente = await _dbContext.Clientes
                    .Include(c => c.Proyectos)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                
                if (cliente == null)
                {
                    TempData["ErrorMessage"] = "El cliente solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los detalles del cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// GET: /Clientes/Create
        /// Muestra el formulario para crear un nuevo cliente
        /// Usado en: <a asp-action="Create">Nuevo Cliente</a>
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await Task.CompletedTask;
            return View();
        }

        /// <summary>
        /// POST: /Clientes/Create
        /// Procesa el formulario de creación de cliente
        /// Usado en: <form asp-action="Create"> con botón submit
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, corrija los errores en el formulario.";
                return View(cliente);
            }

            try
            {
                _dbContext.Clientes.Add(cliente);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"El cliente '{cliente.RazonSocial}' se creó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el cliente: {ex.Message}";
                return View(cliente);
            }
        }

        /// <summary>
        /// GET: /Clientes/Edit/5
        /// Muestra el formulario de edición de un cliente existente
        /// Usado en: <a asp-action="Edit" asp-route-id="@cliente.Id">Editar</a>
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var cliente = await _dbContext.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    TempData["ErrorMessage"] = "El cliente solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el cliente para editar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /Clientes/Edit/5
        /// Procesa el formulario de edición de cliente
        /// Usado en: <form asp-action="Edit"> con botón submit
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                TempData["ErrorMessage"] = "Error de validación: ID de cliente no coincide.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, corrija los errores en el formulario.";
                return View(cliente);
            }

            try
            {
                _dbContext.Clientes.Update(cliente);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"El cliente '{cliente.RazonSocial}' se actualizó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el cliente: {ex.Message}";
                return View(cliente);
            }
        }

        /// <summary>
        /// GET: /Clientes/Delete/5
        /// Muestra la página de confirmación para eliminar un cliente
        /// Usado en: <a asp-action="Delete" asp-route-id="@cliente.Id">Eliminar</a>
        /// </summary>
        [Authorize(Roles = "Lider,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _dbContext.Clientes
                    .Include(c => c.Proyectos)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
                
                if (cliente == null)
                {
                    TempData["ErrorMessage"] = "El cliente solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                if (cliente.Proyectos != null && cliente.Proyectos.Any())
                {
                    TempData["WarningMessage"] = $"Advertencia: El cliente '{cliente.RazonSocial}' tiene {cliente.Proyectos.Count} proyecto(s) asociado(s) que también serán eliminados.";
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el cliente para eliminar: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// POST: /Clientes/Delete/5
        /// Elimina permanentemente un cliente de la base de datos
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
                var cliente = await _dbContext.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    TempData["ErrorMessage"] = "El cliente no existe o ya fue eliminado.";
                    return RedirectToAction(nameof(Index));
                }

                var razonSocial = cliente.RazonSocial;
                _dbContext.Clientes.Remove(cliente);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = $"El cliente '{razonSocial}' se eliminó exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el cliente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
