using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSolutions_program.Models
{
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

        [Required(ErrorMessage = "El nombre del proyecto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe especificar un cliente")]
        public string Cliente { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin Estimada")]
        public DateTime FechaFinEstimada { get; set; }

        [Required]
        [Range(0, 999999999, ErrorMessage = "El presupuesto debe ser un valor positivo")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Presupuesto { get; set; }

        [Required]
        public string Estado { get; set; } = "Planificación";

        [Required]
        public string Prioridad { get; set; } = "Media";

        // Relación: Un proyecto tiene muchas tareas
        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
