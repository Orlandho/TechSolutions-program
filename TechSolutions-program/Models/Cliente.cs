using System.ComponentModel.DataAnnotations;

namespace TechSolutions_program.Models
{
    public class Cliente
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La Razón Social es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Empresa / Razón Social")]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El RUC es obligatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El RUC solo debe contener números")]
        public string RUC { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string CorreoContacto { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        // Relación: Una empresa cliente tiene muchos proyectos con nosotros
        public virtual ICollection<Proyecto> Proyectos { get; set; }

    }
}
