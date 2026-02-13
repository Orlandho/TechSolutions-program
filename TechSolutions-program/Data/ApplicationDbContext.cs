using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Models;

namespace TechSolutions_program.Data
{

    // CAMBIO IMPORTANTE: Heredamos de IdentityDbContext<ApplicationUser>
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {

        /*
         * ====================================================================================
         * CAPA DE ACCESO A DATOS: CONTEXTO DE BASE DE DATOS
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Caso Integrador: "Sistema web centralizado" con "base de datos relacional".
         * - Ítem 3: Patrones de Diseño (Singleton para conexión).
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Actúa como la unidad de trabajo (Unit of Work) y puente con SQL Server.
         * * PATRONES Y SEGURIDAD:
         * 1. Patrón Singleton: Gestionado por la inyección de dependencias de ASP.NET Core
         * (Scoped), garantizando una única instancia de conexión por petición HTTP.
         * 2. Seguridad Anti-Inyección SQL: Utiliza Entity Framework Core para parametrizar
         * automáticamente todas las consultas, cumpliendo la Sección 5 del informe.
         * ====================================================================================
         */

        // CAMBIO CRÍTICO: Heredamos de IdentityDbContext<Usuario>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        // No necesitas 'DbSet<Usuario>' porque Identity ya lo incluye internamente
    }
}
