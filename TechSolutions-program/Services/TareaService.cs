using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Data;
using TechSolutions_program.Models;

namespace TechSolutions_program.Services
{
    public class TareaService : ITareaService
    {
        /*
         * ====================================================================================
         * CAPA DE LÓGICA DE NEGOCIO: GESTOR DE TAREAS
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Modelo de Análisis: Clase de Control "GestorTareas".
         * - CU-02: Gestionar Tareas y CU-03: Actualizar Avance.
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Centraliza las reglas de negocio para la operatividad diaria de los desarrolladores.
         * * FUNCIONES CLAVE:
         * 1. Asignación: Verifica que la tarea se asigne a un usuario existente y con rol 'Desarrollador'.
         * 2. Control de Estado: Gestiona la transición de estados (Pendiente -> En Progreso -> Finalizado).
         * 3. Cálculo de Impacto: Al cerrar una tarea, dispara el recálculo del avance general 
         * del proyecto asociado.
         * ====================================================================================
         */

        private readonly ApplicationDbContext _dbContext;

        public TareaService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            return await _dbContext.Tareas.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Tarea>> GetTareasPorResponsableAsync(string responsableId)
        {
            return await _dbContext.Tareas.AsNoTracking()
                .Where(t => t.Responsable == responsableId)
                .ToListAsync();
        }

        public async Task<Tarea?> GetByIdAsync(int id)
        {
            return await _dbContext.Tareas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CrearAsync(Tarea tarea)
        {
            _dbContext.Tareas.Add(tarea);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Tarea tarea)
        {
            _dbContext.Tareas.Update(tarea);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var tarea = await _dbContext.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                return;
            }

            _dbContext.Tareas.Remove(tarea);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var tarea = await _dbContext.Tareas.FirstOrDefaultAsync(t => t.Id == id);
            if (tarea == null)
            {
                return;
            }

            tarea.Estado = nuevoEstado;
            await _dbContext.SaveChangesAsync();
        }
    }
}
