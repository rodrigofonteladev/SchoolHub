using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Shift
{
    public class RegisterShiftViewModel
    {
        [Required(ErrorMessage = "El nombre del turno es requerido")]
        [StringLength(15)]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;
    }
}
