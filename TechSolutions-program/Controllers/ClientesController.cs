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
            var clientes = await _dbContext.Clientes.AsNoTracking().ToListAsync();
            return View(clientes);
        }

        /// <summary>
        /// GET: /Clientes/Details/5
        /// Muestra los detalles de un cliente específico
        /// Usado en: <a asp-action="Details" asp-route-id="@cliente.Id">Ver Detalles</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        /// <summary>
        /// GET: /Clientes/Create
        /// Muestra el formulario para crear un nuevo cliente
        /// Usado en: <a asp-action="Create">Nuevo Cliente</a>
        /// </summary>
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            _dbContext.Clientes.Add(cliente);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /Clientes/Edit/5
        /// Muestra el formulario de edición de un cliente existente
        /// Usado en: <a asp-action="Edit" asp-route-id="@cliente.Id">Editar</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        /// <summary>
        /// POST: /Clientes/Edit/5
        /// Procesa el formulario de edición de cliente
        /// Usado en: <form asp-action="Edit"> con botón submit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            _dbContext.Clientes.Update(cliente);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// GET: /Clientes/Delete/5
        /// Muestra la página de confirmación para eliminar un cliente
        /// Usado en: <a asp-action="Delete" asp-route-id="@cliente.Id">Eliminar</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        /// <summary>
        /// POST: /Clientes/Delete/5
        /// Elimina permanentemente un cliente de la base de datos
        /// Usado en: <form asp-action="Delete"> con botón "Confirmar Eliminación"
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _dbContext.Clientes.Remove(cliente);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
