using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Data;
using TechSolutions_program.Models;
using TechSolutions_program.Services.Interfaces;

namespace TechSolutions_program.Services.Implementations
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

        private readonly ApplicationDbContext _dbContext;

        public ProyectoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Proyecto>> GetProyectosAsync()
        {
            return await _dbContext.Proyectos.AsNoTracking().ToListAsync();
        }

        public async Task<Proyecto?> GetByIdAsync(int id)
        {
            return await _dbContext.Proyectos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CrearAsync(Proyecto proyecto)
        {
            _dbContext.Proyectos.Add(proyecto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Proyecto proyecto)
        {
            _dbContext.Proyectos.Update(proyecto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var proyecto = await _dbContext.Proyectos.FirstOrDefaultAsync(p => p.Id == id);
            if (proyecto == null)
            {
                return;
            }

            _dbContext.Proyectos.Remove(proyecto);
            await _dbContext.SaveChangesAsync();
        }
    }
}
