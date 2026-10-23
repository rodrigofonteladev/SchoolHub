using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Rating
{
    public class RegisterRatingViewModel
    {
        [Required(ErrorMessage = "La matrícula es requerida")]
        [Display(Name = "Matrícula")]
        public int? EnrollmentId { get; set; }

        [Required(ErrorMessage = "La asignación de clase es requerida")]
        [Display(Name = "Asignación de clase")]
        public int? ClassAsignmentId { get; set; }

        [Range(0, 20, ErrorMessage = "La nota debe estar entre 0 y 20")]
        [Display(Name = "Examen 1")]
        public decimal Exam1 { get; set; }

        [Range(0, 20, ErrorMessage = "La nota debe estar entre 0 y 20")]
        [Display(Name = "Examen 2")]
        public decimal Exam2 { get; set; }

        [Range(0, 20, ErrorMessage = "La nota debe estar entre 0 y 20")]
        [Display(Name = "Examen 3")]
        public decimal Exam3 { get; set; }

        public IEnumerable<SelectListItem> Enrollments { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ClassAsignments { get; set; } = new List<SelectListItem>();
    }
}
