using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSolutions_program.Models
{
    public class Tarea
    {

        /*
         * ====================================================================================
         * ENTIDAD DE DOMINIO: TAREA
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Casos de Uso CU-02 (Gestionar Tareas) y CU-03 (Actualizar Avance).
         * - Diagrama de Clases de Análisis (Relación Proyecto-Tarea).
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Representa la unidad de trabajo asignable a un 'Desarrollador'. Permite el 
         * seguimiento granular del avance del proyecto requerido por la gerencia.
         * * FUNCIONALIDAD:
         * - Relación: Implementa la cardinalidad de 'Uno a Muchos' con Proyecto.
         * - Trazabilidad: Almacena el estado (Pendiente, En Progreso, Terminado) que 
         * alimentará el Dashboard de control (CU-05).
         * ====================================================================================
         */

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción de la tarea es necesaria")]
        public string Descripcion { get; set; }

        [Required]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Progreso, Terminado

        [Required]
        public string Prioridad { get; set; } = "Media";

        public string Responsable { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaLimite { get; set; }

        // Relación con Proyecto: Cada tarea pertenece a un Proyecto
        [Required]
        public int ProyectoId { get; set; }

        [ForeignKey("ProyectoId")]
        public virtual Proyecto Proyecto { get; set; }
    }
}
