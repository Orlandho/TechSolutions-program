namespace TechSolutions_program.Models
{
    /// <summary>
    /// ViewModel utilizado para mostrar indicadores clave en el Dashboard
    /// Contiene métricas agregadas del estado del sistema
    /// Usado en: SeguimientoController.Index() para llenar la vista del dashboard
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// Número total de proyectos en el sistema
        /// Mostrado en: Dashboard como métrica principal
        /// </summary>
        public int TotalProyectos { get; set; }

        /// <summary>
        /// Suma de presupuestos de todos los proyectos
        /// Mostrado en: Dashboard para visualización financiera
        /// </summary>
        public decimal PresupuestoTotal { get; set; }

        /// <summary>
        /// Cantidad de tareas con estado "Pendiente"
        /// Mostrado en: Dashboard para control de trabajo por realizar
        /// </summary>
        public int TareasPendientes { get; set; }

        /// <summary>
        /// Cantidad de tareas con estado "Terminado" o "Finalizado"
        /// Mostrado en: Dashboard para medir productividad
        /// </summary>
        public int TareasCompletadas { get; set; }
    }
}
