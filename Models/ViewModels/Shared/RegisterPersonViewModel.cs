using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Shared
{
    public abstract class RegisterPersonViewModel
    {
        [Required(ErrorMessage = "El nombre completo es requerido")]
        [Display(Name = "Nombres Completos")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "El apellido paterno es requerido")]
        [Display(Name = "Apellido Paterno")]
        public string PaternalSurname { get; set; } = null!;

        [Required(ErrorMessage = "El apellido materno es requerido")]
        [Display(Name = "Apellido Materno")]
        public string MaternalSurname { get; set; } = null!;

        [Required(ErrorMessage = "El número de documento es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número de documento debe contener solo números")]
        [Display(Name = "Número de Documento")]
        public string DocumentNumber { get; set; } = null!;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es requerida")]
        [Display(Name = "Dirección")]
        public string Address { get; set; } = null!;
    }
}
