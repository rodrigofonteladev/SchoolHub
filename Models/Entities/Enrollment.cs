using SchoolHub.Common.Enums;

namespace SchoolHub.Models.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public DateOnly EnrolledAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
        public int EnrolledYear { get; set; } = DateTime.Now.Year;
        public int StudentId { get; set; }
        public int SectionId { get; set; }
        public int ReasonEnrollmentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Student Student { get; set; } = null!;
        public Section Section { get; set; } = null!;
        public ReasonEnrollment ReasonEnrollment { get; set; } = null!;
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
