using SchoolHub.Common.Enums;

namespace SchoolHub.Models.ViewModels.Enrollment
{
    public class ListEnrollmentViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = null!;
        public string SectionName { get; set; } = null!;
        public string GradeName { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public int EnrolledYear { get; set; }
        public DateOnly EnrolledAt { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
