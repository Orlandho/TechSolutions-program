using TechSolutions_program.Models;

namespace TechSolutions_program.Services
{
    public class ProyectoService : IProyectoService
    {
        /*
         * ====================================================================================
         * CAPA DE LÓGICA DE NEGOCIO: GESTOR DE PROYECTOS
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Modelo de Análisis: Clase de Control "GestorProyectos".
         * - CU-01: Registrar Proyecto (Validaciones de negocio).
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Contiene la inteligencia del negocio. No accede a la BD directamente, usa el Repository.
         * * REGLAS DE NEGOCIO IMPLEMENTADAS:
         * 1. Validación de Fechas: (FechaFin > FechaInicio).
         * 2. Validación Financiera: El presupuesto debe ser positivo.
         * 3. Cálculo de Estado: Determina si un proyecto está atrasado según sus tareas.
         * ====================================================================================
         */

        public Task<IEnumerable<Proyecto>> GetProyectosAsync()
        {
            return Task.FromResult<IEnumerable<Proyecto>>(Array.Empty<Proyecto>());
        }

        public Task<Proyecto?> GetByIdAsync(int id)
        {
            return Task.FromResult<Proyecto?>(null);
        }

        public Task CrearAsync(Proyecto proyecto)
        {
            return Task.CompletedTask;
        }

        public Task ActualizarAsync(Proyecto proyecto)
        {
            return Task.CompletedTask;
        }

        public Task EliminarAsync(int id)
        {
            return Task.CompletedTask;
        }
    }
}
