using Microsoft.AspNetCore.Mvc;

namespace TechSolutions_program.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
