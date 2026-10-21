using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Grade
{
    public class RegisterGradeViewModel
    {
        [Required(ErrorMessage = "The grade name is required")]
        [StringLength(5)]
        [Display(Name = "Nombre del grado")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The level is required")]
        [Display(Name = "Nivel Educativo")]
        public int? LevelId { get; set; }

        public IEnumerable<SelectListItem> Levels { get; set; } = new List<SelectListItem>();
    }
}