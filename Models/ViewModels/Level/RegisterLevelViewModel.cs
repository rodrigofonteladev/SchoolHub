using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Level
{
    public class RegisterLevelViewModel
    {
        [Required(ErrorMessage = "The level name is required")]
        [StringLength(20)]
        [Display(Name = "Nombre del nivel educativo")]
        public string Name { get; set; } = null!;
    }
}