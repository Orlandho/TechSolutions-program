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

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string Cliente { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFinEstimada { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Presupuesto { get; set; }

        public string Estado { get; set; } = "Planificación";
        public string Prioridad { get; set; } = "Media";

    }
}
