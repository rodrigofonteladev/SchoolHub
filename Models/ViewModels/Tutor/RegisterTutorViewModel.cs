using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolHub.Common.Enums;
using SchoolHub.Models.ViewModels.Shared;

namespace SchoolHub.Models.ViewModels.Tutor
{
    public class RegisterTutorViewModel : RegisterPersonViewModel
    {
        [Required(ErrorMessage = "The document type is required")]
        [Display(Name = "Document Type")]
        public int? DocumentTypeId { get; set; }

        [Required(ErrorMessage = "The relationship is required")]
        public TutorType? Relationship { get; set; }

        [Required(ErrorMessage = "The student is required")]
        [Display(Name = "Student")]
        public int? StudentId { get; set; }

        public IEnumerable<SelectListItem> DocumentTypes { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Students { get; set; } = new List<SelectListItem>();
    }
}
