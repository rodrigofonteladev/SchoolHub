using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Schedule
{
    public class RegisterScheduleViewModel
    {
        [Required(ErrorMessage = "La hora de inicio es requerida")]
        [Display(Name = "Hora de inicio")]
        public TimeOnly? StartTime { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida")]
        [Display(Name = "Hora de fin")]
        public TimeOnly? EndTime { get; set; }

        [Required(ErrorMessage = "El turno es requerido")]
        [Display(Name = "Turno")]
        public int? ShiftId { get; set; }

        [Required(ErrorMessage = "El día es requerido")]
        [Display(Name = "Día")]
        public int? DayWeekId { get; set; }

        public IEnumerable<SelectListItem> Shifts { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Days { get; set; } = new List<SelectListItem>();
    }
}
