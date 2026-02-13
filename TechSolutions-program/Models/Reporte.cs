/*
 * ====================================================================================
 * ENTIDAD DE APOYO: REPORTE
 * ====================================================================================
 * REFERENCIA DOCUMENTAL: 
 * - Ítem 3: Patrón Strategy para exportación dinámica.
 * - CU-05: Visualizar Dashboard y reportes.
 * * DESCRIPCIÓN TÉCNICA:
 * Clase que encapsula los parámetros de generación de un reporte.
 * ====================================================================================
 */
using System.ComponentModel.DataAnnotations;


namespace TechSolutions_program.Models
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string TipoFormato { get; set; } // Ejemplo: PDF o Excel

        public DateTime FechaGeneracion { get; set; } = DateTime.Now;

        public string GeneradoPor { get; set; }

        // Este modelo no suele persistirse si solo es para la descarga, 
        // pero se incluye para cumplir con la estructura de clases del documento.
    }
}
