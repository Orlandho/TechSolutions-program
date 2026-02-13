using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechSolutions_program.Models
{
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


        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; }

        public string? CodigoEmpleado { get; set; } // Opcional

        // Aquí podríamos relacionar al usuario con sus tareas asignadas
        public virtual ICollection<Tarea> TareasAsignadas { get; set; }

    }
}
