using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public string RazonSocial { get; set; } = string.Empty;

        /// <summary>
        /// Registro Único de Contribuyentes (identificador tributario en Perú)
        /// Usado en: Identificación única del cliente y reportes fiscales
        /// </summary>
        [Required(ErrorMessage = "El RUC es obligatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El RUC solo debe contener números")]
        public string RUC { get; set; } = string.Empty;

        /// <summary>
        /// Correo electrónico de contacto del cliente
        /// Usado en: Comunicaciones y notificaciones del proyecto
        /// </summary>
        [Required(ErrorMessage = "El correo de contacto es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string EmailContacto { get; set; } = string.Empty;

        /// <summary>
        /// Número de teléfono de contacto
        /// Usado en: Comunicaciones directas con el cliente
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Dirección física de la empresa cliente
        /// Usado en: Información de contacto y documentación legal
        /// </summary>
        public string? Direccion { get; set; }

        /// <summary>
        /// Colección de proyectos asociados a este cliente
        /// Usado en: Vista de detalles del cliente para listar sus proyectos
        /// NOTA: Esta propiedad NO se valida en formularios
        /// </summary>
        [BindNever] // No vincular en model binding para evitar validación
        public virtual ICollection<Proyecto>? Proyectos { get; set; }

    }
}
