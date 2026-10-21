namespace SchoolHub.Models.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public decimal Exam1 { get; set; }
        public decimal Exam2 { get; set; }
        public decimal Exam3 { get; set; }
        public int EnrollmentId { get; set; }
        public int ClassAsignmentId { get; set; }

        public Enrollment Enrollment { get; set; } = null!;
        public ClassAsignment ClassAsignment { get; set; } = null!;
    }
}
