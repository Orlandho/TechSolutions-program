namespace TechSolutions_program.Data.Repositories
{
    public interface IProyectoRepository
    {

        /*
         * ====================================================================================
         * CONTRATO DE INTERFAZ: REPOSITORIO DE PROYECTOS
         * ====================================================================================
         * REFERENCIA DOCUMENTAL: 
         * - Diagrama de Paquetes: Interfaz que define las operaciones de datos.
         * - Principio de Diseño: Inversión de Dependencias (DIP) y Desacoplamiento.
         * * DESCRIPCIÓN TÉCNICA Y CONTEXTO:
         * Define el contrato estricto de las operaciones CRUD disponibles para la entidad Proyecto.
         * * JUSTIFICACIÓN ARQUITECTÓNICA:
         * Obliga a que cualquier implementación de persistencia (SQL, Oracle, Archivos) cumpla
         * con estos métodos. Permite realizar pruebas unitarias (Mocking) del 'ProyectoService'
         * sin necesitar la base de datos real, elevando la calidad del software.
         * ====================================================================================
         */

    }
}
