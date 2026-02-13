using System.ComponentModel.DataAnnotations;

namespace TechSolutions_program.Models
{
    /// <summary>
    /// Entidad que representa un cliente empresarial de TechSolutions
    /// Almacena información de contacto y datos legales de la empresa
    /// Usada en: ClientesController para gestión CRUD de clientes
    /// </summary>
    public class Cliente
    {

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre legal de la empresa cliente
        /// Se muestra en: Listados, formularios y como referencia en proyectos
        /// </summary>
        [Required(ErrorMessage = "La Razón Social es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Empresa / Razón Social")]
        public string RazonSocial { get; set; }

        /// <summary>
        /// Registro Único de Contribuyentes (identificador tributario en Perú)
        /// Usado en: Identificación única del cliente y reportes fiscales
        /// </summary>
        [Required(ErrorMessage = "El RUC es obligatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El RUC solo debe contener números")]
        public string RUC { get; set; }

        /// <summary>
        /// Correo electrónico de contacto del cliente
        /// Usado en: Comunicaciones y notificaciones del proyecto
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string EmailContacto { get; set; }

        /// <summary>
        /// Número de teléfono de contacto
        /// Usado en: Comunicaciones directas con el cliente
        /// </summary>
        public string Telefono { get; set; }

        /// <summary>
        /// Dirección física de la empresa cliente
        /// Usado en: Información de contacto y documentación legal
        /// </summary>
        public string Direccion { get; set; }

        /// <summary>
        /// Colección de proyectos asociados a este cliente
        /// Usado en: Vista de detalles del cliente para listar sus proyectos
        /// </summary>
        // Relación: Una empresa cliente tiene muchos proyectos con nosotros
        public virtual ICollection<Proyecto> Proyectos { get; set; }

    }
}
