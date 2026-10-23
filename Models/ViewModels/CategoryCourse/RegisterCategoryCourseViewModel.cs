using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.CategoryCourse
{
    public class RegisterCategoryCourseViewModel
    {
        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [StringLength(256)]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }
    }
}
