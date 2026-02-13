/*
 * ====================================================================================
 * CAPA DE NEGOCIO: SERVICIO DE REPORTERÍA (CONTEXTO STRATEGY)
 * ====================================================================================
 * REFERENCIA DOCUMENTAL: 
 * - Ítem 3: Patrones de Diseño -> Patrón Strategy.
 * - CU-05: Exportación de datos para la toma de decisiones.
 * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
 * Motor de generación de reportes que implementa el principio Open/Closed (SOLID).
 * Actúa como el 'Context' en el patrón Strategy.
 * * DINÁMICA DEL PATRÓN:
 * 1. Desacoplamiento: Este servicio NO sabe cómo crear un PDF o un Excel.
 * 2. Inyección de Estrategia: Recibe una implementación de 'IReporteStrategy'
 * (ya sea PdfStrategy o ExcelStrategy) y ejecuta la generación sin modificar su código.
 * 3. Singleton/Scoped: Diseñado para ser eficiente en memoria al procesar grandes
 * volúmenes de datos de proyectos y tareas antes de exportarlos.
 * ====================================================================================
 */


namespace TechSolutions_program.Services
{
    public class ReporteService
    {
    }
}
