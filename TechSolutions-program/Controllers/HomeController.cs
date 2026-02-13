using Microsoft.AspNetCore.Mvc;

namespace TechSolutions_program.Controllers
{
    /// <summary>
    /// Controlador principal para la página de inicio del sistema
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// GET: /Home/Index o /
        /// Muestra la página de inicio principal del sistema TechSolutions
        /// Usado en: <a asp-controller="Home" asp-action="Index">Inicio</a>
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
