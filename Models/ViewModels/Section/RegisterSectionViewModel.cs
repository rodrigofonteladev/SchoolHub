using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Section
{
    public class RegisterSectionViewModel
    {
        [Required(ErrorMessage = "The section name is required")]
        [StringLength(1)]
        [Display(Name = "Nombre de la sección")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The grade is required")]
        [Display(Name = "Grado")]
        public int? GradeId { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; } = new List<SelectListItem>();
    }
}