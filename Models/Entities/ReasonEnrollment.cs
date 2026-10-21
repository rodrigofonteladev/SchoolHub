namespace SchoolHub.Models.Entities
{
    public class ReasonEnrollment
    {
        public int Id { get; set; }
        public required string Description { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
