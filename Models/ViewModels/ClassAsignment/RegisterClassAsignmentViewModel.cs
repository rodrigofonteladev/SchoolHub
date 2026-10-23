using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.ClassAsignment
{
    public class RegisterClassAsignmentViewModel
    {
        [Range(2000, 2100, ErrorMessage = "El año no es válido")]
        [Display(Name = "Año asignado")]
        public int AssignedYear { get; set; } = DateTime.Now.Year;

        [Required(ErrorMessage = "El docente es requerido")]
        [Display(Name = "Docente")]
        public int? TeacherId { get; set; }

        [Required(ErrorMessage = "El curso es requerido")]
        [Display(Name = "Curso")]
        public int? CourseId { get; set; }

        [Required(ErrorMessage = "El horario es requerido")]
        [Display(Name = "Horario")]
        public int? ScheduleId { get; set; }

        [Required(ErrorMessage = "La sección es requerida")]
        [Display(Name = "Sección")]
        public int? SectionId { get; set; }

        public IEnumerable<SelectListItem> Teachers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Schedules { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Sections { get; set; } = new List<SelectListItem>();
    }
}
