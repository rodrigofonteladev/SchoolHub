using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolHub.Models.ViewModels.Shared;
using SchoolHub.Models.ViewModels.Tutor;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Student
{
    public class RegisterStudentViewModel : RegisterPersonViewModel
    {
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateOnly? Birthday { get; set; }

        [Required(ErrorMessage = "El tipo de documento es requerido")]
        [Display(Name = "Tipo de Documento")]
        public int? DocumentTypeId { get; set; }

        [Required(ErrorMessage = "El número de teléfono es requerido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número de teléfono debe contener solo números")]
        [Display(Name = "Número de Teléfono")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "El nivel educativo es requerido")]
        [Display(Name = "Nivel Educativo")]
        public int? SectionId { get; set; }

        [Required(ErrorMessage = "El motivo de matrícula es requerido")]
        [Display(Name = "Motivo de Matrícula")]
        public int? ReasonEnrollmentId { get; set; }

        [MinLength(1, ErrorMessage = "Se requiere al menos un tutor(a)")]
        public List<RegisterTutorViewModel> Tutors { get; set; } = new List<RegisterTutorViewModel>();

        public IEnumerable<SelectListItem> DocumentTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> Sections { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> ReasonEnrollments { get; set; } = new List<SelectListItem>();
    }
}
