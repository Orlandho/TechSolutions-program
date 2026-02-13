using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TechSolutions_program.Models
{
    /// <summary>
    /// Entidad que representa una tarea asignada dentro de un proyecto
    /// Permite el seguimiento del avance y la asignación de responsabilidades
    /// Usada en: TareasController para gestionar asignaciones y cambios de estado
    /// </summary>
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

        /// <summary>
        /// Descripción detallada de la tarea a realizar
        /// Se muestra en: Listados de tareas y formularios de edición
        /// </summary>
        [Required(ErrorMessage = "La descripción de la tarea es necesaria")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Estado actual de la tarea: Pendiente, En Progreso, Terminado
        /// Usado en: Dashboard para calcular avance y en botones de cambio de estado
        /// Cambiado desde: TareasController.CambiarEstado()
        /// </summary>
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Progreso, Terminado

        /// <summary>
        /// Prioridad de la tarea: Baja, Media, Alta
        /// Usado en: Ordenamiento y filtros en vistas de tareas
        /// </summary>
        [Required(ErrorMessage = "La prioridad es obligatoria")]
        public string Prioridad { get; set; } = "Media";

        /// <summary>
        /// ID del usuario (desarrollador) responsable de la tarea
        /// Usado en: Filtro de MisTareas para mostrar solo las tareas del usuario actual
        /// </summary>
        public string? ResponsableId { get; set; }

        /// <summary>
        /// Fecha límite para completar la tarea
        /// Usado en: Dashboard para identificar tareas con retraso
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? FechaLimite { get; set; }

        /// <summary>
        /// ID del proyecto al que pertenece esta tarea
        /// Usado en: Relación con la entidad Proyecto y filtros por proyecto
        /// </summary>
        [Required(ErrorMessage = "Debe especificar un proyecto")]
        public int ProyectoId { get; set; }

        /// <summary>
        /// Navegación al proyecto padre
        /// Usado en: Vistas de detalle para mostrar información del proyecto asociado
        /// NOTA: Esta propiedad NO se valida en formularios, solo ProyectoId
        /// </summary>
        [ForeignKey("ProyectoId")]
        [BindNever] // No vincular en model binding para evitar validación
        public virtual Proyecto? Proyecto { get; set; }
    }
}
