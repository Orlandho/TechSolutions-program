namespace TechSolutions_program.Services.Strategies
{
    public class ReporteResultado
    {
        public ReporteResultado(Stream contenido, string contentType, string nombreArchivo)
        {
            Contenido = contenido;
            ContentType = contentType;
            NombreArchivo = nombreArchivo;
        }

        public Stream Contenido { get; }
        public string ContentType { get; }
        public string NombreArchivo { get; }
    }
}
