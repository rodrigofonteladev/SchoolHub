using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.DayWeek
{
    public class RegisterDayWeekViewModel
    {
        [Required(ErrorMessage = "El nombre del día es requerido")]
        [StringLength(10)]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;
    }
}
