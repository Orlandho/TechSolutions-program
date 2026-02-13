using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Models;

namespace TechSolutions_program.Data
{
    public class ApplicationDbContext : IdentityDbContext
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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ESTO ES VITAL: Le dice a la BD que cree la tabla Proyectos
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }

    }
}
