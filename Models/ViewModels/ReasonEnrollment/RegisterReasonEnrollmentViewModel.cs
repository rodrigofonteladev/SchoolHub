using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.ReasonEnrollment
{
    public class RegisterReasonEnrollmentViewModel
    {
        [Required(ErrorMessage = "El motivo de matrícula es requerido")]
        [StringLength(256)]
        [Display(Name = "Motivo de matrícula")]
        public string Description { get; set; } = null!;
    }
}