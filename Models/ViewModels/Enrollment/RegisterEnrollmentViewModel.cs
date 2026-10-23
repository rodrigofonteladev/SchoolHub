using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Enrollment
{
    public class RegisterEnrollmentViewModel
    {
        [Required(ErrorMessage = "El estudiante es requerido")]
        [Display(Name = "Estudiante")]
        public int? StudentId { get; set; }

        [Required(ErrorMessage = "La sección es requerida")]
        [Display(Name = "Sección")]
        public int? SectionId { get; set; }

        [Required(ErrorMessage = "El motivo es requerido")]
        [Display(Name = "Motivo de matrícula")]
        public int? ReasonEnrollmentId { get; set; }

        [Range(2000, 2100, ErrorMessage = "El año no es válido")]
        [Display(Name = "Año de matrícula")]
        public int EnrolledYear { get; set; } = DateTime.Now.Year;

        public IEnumerable<SelectListItem> Students { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Sections { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Reasons { get; set; } = new List<SelectListItem>();
    }
}
