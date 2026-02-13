using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSolutions_program.Models
{
    /// <summary>
    /// Entidad que representa un proyecto de software en TechSolutions
    /// Almacena información crítica: presupuesto, fechas, cliente, estado y prioridad
    /// Usada en: ProyectosController para operaciones CRUD y en vistas de proyectos
    /// </summary>
    public class Proyecto
    {

        /*
         * ====================================================================================
         * ENTIDAD DE DOMINIO: PROYECTO
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Caso Integrador (Párrafo 2): Requisitos de información del proyecto.
         * - Ejercicio 10: Diseño de tablas y relaciones.
         * - Sección 5 (Seguridad): Validación de Datos para integridad.
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Esta clase representa la tabla principal 'PROYECTO' en la base de datos SQL Server.
         * Cumple con el requisito de centralizar la gestión que antes se hacía en Excel.
         * * REGLAS IMPLEMENTADAS:
         * 1. Validación de Datos: Uso de DataAnnotations ([Required], [StringLength]) para 
         * prevenir datos corruptos, alineado con las estrategias de seguridad del informe.
         * 2. Mapeo de Campos: Incluye estrictamente los campos solicitados: Presupuesto, 
         * Fechas (Inicio/Fin), Cliente, Estado y Prioridad.
         * ====================================================================================
         */

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre descriptivo del proyecto
        /// Se muestra en: Listas, formularios y detalles del proyecto
        /// </summary>
        [Required(ErrorMessage = "El nombre del proyecto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; }

        /// <summary>
        /// Identificación del cliente que solicita el proyecto
        /// Usado en: Filtros y reportes de proyectos por cliente
        /// </summary>
        [Required(ErrorMessage = "Debe especificar un cliente")]
        public string Cliente { get; set; }

        /// <summary>
        /// Fecha de inicio del proyecto
        /// Usado en: Cálculo de duración y validación de cronograma
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha estimada de finalización del proyecto
        /// Usado en: Dashboard para determinar proyectos con retraso
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin Estimada")]
        public DateTime FechaFinEstimada { get; set; }

        /// <summary>
        /// Presupuesto total asignado al proyecto
        /// Usado en: Reportes financieros y cálculos de rentabilidad
        /// </summary>
        [Required]
        [Range(0, 999999999, ErrorMessage = "El presupuesto debe ser un valor positivo")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Presupuesto { get; set; }

        /// <summary>
        /// Estado actual del proyecto: Planificación, En Desarrollo, Finalizado
        /// Usado en: Dashboard y filtros de proyectos
        /// </summary>
        [Required]
        public string Estado { get; set; } = "Planificación";

        /// <summary>
        /// Prioridad del proyecto: Baja, Media, Alta
        /// Usado en: Ordenamiento y gestión de recursos
        /// </summary>
        [Required]
        public string Prioridad { get; set; } = "Media";

        /// <summary>
        /// Colección de tareas asociadas a este proyecto
        /// Usado en: Detalles del proyecto para mostrar tareas asignadas
        /// </summary>
        // Relación: Un proyecto tiene muchas tareas
        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
