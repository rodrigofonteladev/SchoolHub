using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Course
{
    public class RegisterCourseViewModel
    {
        [Required(ErrorMessage = "El nombre del curso es requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [StringLength(256)]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Range(1, 40, ErrorMessage = "Las horas semanales deben estar entre 1 y 40")]
        [Display(Name = "Horas semanales")]
        public int WeeklyHours { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [Display(Name = "Categoría")]
        public int? CategoryCourseId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
