using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechSolutions_program.Models
{
    /// <summary>
    /// Entidad de usuario del sistema basada en ASP.NET Core Identity
    /// Representa a los actores: Líder, Desarrollador o Administrador
    /// Usada en: AutenticacionController para login y en TareasController para asignaciones
    /// </summary>
    public class Usuario : IdentityUser
    {

        /*
         * ====================================================================================
         * ENTIDAD DE SEGURIDAD: USUARIO (ACTOR)
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Sección 5: Consideraciones de Seguridad (Autenticación y Autorización).
         * - Definición de Actores: Líder de Proyecto, Desarrollador, Administrador.
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Extiende la identidad base del sistema para manejar los accesos.
         * * IMPLEMENTACIÓN DE SEGURIDAD:
         * 1. RBAC (Role-Based Access Control): Es la base para diferenciar permisos entre
         * 'Líderes' (Edición total) y 'Desarrolladores' (Solo lectura/Avance).
         * 2. Hashing: Soporta el requisito de almacenar contraseñas encriptadas.
         * ====================================================================================
         */

        /// <summary>
        /// Nombre completo del usuario para identificación en el sistema
        /// Mostrado en: Encabezado de la aplicación, listados de asignaciones
        /// </summary>
        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        /// <summary>
        /// Código de empleado único (opcional)
        /// Usado en: Identificación interna de recursos humanos
        /// </summary>
        public string? CodigoEmpleado { get; set; } // Opcional

        /// <summary>
        /// Colección de tareas asignadas a este usuario (desarrollador)
        /// Usada en: Vista MisTareas para mostrar solo las tareas del usuario actual
        /// </summary>
        // Aquí podríamos relacionar al usuario con sus tareas asignadas
        public virtual ICollection<Tarea> TareasAsignadas { get; set; }

    }
}
