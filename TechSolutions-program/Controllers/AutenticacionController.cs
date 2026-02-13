using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechSolutions_program.Models;

namespace TechSolutions_program.Controllers
{
    /*
     * ====================================================================================
     * CAPA DE PRESENTACIÓN: CONTROLADOR DE AUTENTICACIÓN Y ACCESO
     * ====================================================================================
     * REFERENCIA DOCUMENTAL: 
     * - Sección 5: Consideraciones de Seguridad (Autenticación y Autorización).
     * - Requisito Funcional: Gestión de Roles (Líder, Desarrollador, Administrador).
     * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
     * Actúa como el punto de entrada de seguridad del sistema TechSolutions. Gestiona
     * el ciclo de vida de la sesión del usuario utilizando ASP.NET Core Identity.
     * * IMPLEMENTACIÓN DE SEGURIDAD SEGÚN EL DOCUMENTO:
     * 1. RBAC (Role-Based Access Control): Implementa la lógica para validar los 
     * permisos diferenciados que impiden que un Desarrollador acceda a funciones 
     * administrativas o financieras del Líder.
     * 2. Protección de Identidad: Utiliza el Middleware de Identity para asegurar
     * que las contraseñas nunca viajen o se almacenen en texto plano, cumpliendo 
     * con el estándar de hashing exigido en el informe técnico.
     * 3. Prevención de Ataques: Incluye tokens Anti-Forgery para mitigar riesgos
     * de CSRF en los formularios de inicio de sesión.
     * ====================================================================================
     */
    public class AutenticacionController : Controller
    {
        public const string RolLider = "Lider";
        public const string RolDesarrollador = "Desarrollador";
        public const string RolAdministrador = "Administrador";

        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public AutenticacionController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// GET: /Autenticacion/Login
        /// Muestra el formulario de inicio de sesión
        /// Usado en: <a asp-controller="Autenticacion" asp-action="Login">Iniciar Sesión</a>
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await Task.CompletedTask;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// POST: /Autenticacion/Login
        /// Procesa el intento de inicio de sesión del usuario
        /// Usado en: <form asp-action="Login"> con campos de email y password
        /// Redirige a la página solicitada o a /Proyectos/Index si login exitoso
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            // Seguridad crítica: error genérico para evitar revelar existencia del usuario.
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                // CAMBIADO: Redirigir al Dashboard después del login exitoso
                return RedirectToAction("Index", "Seguimiento");
            }

            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido");
            return View();
        }

        /// <summary>
        /// POST: /Autenticacion/Logout
        /// Cierra la sesión del usuario actual
        /// Usado en: <form asp-action="Logout"> con botón "Cerrar Sesión"
        /// Típicamente en: _LoginPartial.cshtml
        /// </summary>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Autenticacion");
        }

        /// <summary>
        /// GET: /Autenticacion/AccessDenied
        /// Muestra la página de acceso denegado cuando un usuario no tiene permisos
        /// Redirigido automáticamente por [Authorize(Roles = "...")] cuando fallan permisos
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            await Task.CompletedTask;
            TempData["ErrorMessage"] = "No tienes permisos para acceder a esta funcionalidad. Contacta al administrador si necesitas acceso.";
            return View();
        }
    }
}
