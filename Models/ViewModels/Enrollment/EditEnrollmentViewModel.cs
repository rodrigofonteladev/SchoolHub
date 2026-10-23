using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolHub.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Enrollment
{
    public class EditEnrollmentViewModel
    {
        public string StudentName { get; set; } = null!;
        public int EnrolledYear { get; set; }
        [Required] public int? SectionId { get; set; }
        [Required] public int? ReasonEnrollmentId { get; set; }
        [Required] public EnrollmentStatus Status { get; set; }
        public IEnumerable<SelectListItem> Sections { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Reasons { get; set; } = new List<SelectListItem>();
    }
}
