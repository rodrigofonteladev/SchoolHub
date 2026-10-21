namespace SchoolHub.Models.Entities
{
    public class Section
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int GradeId { get; set; }
        public int LimitStudents { get; set; }

        public Grade Grade { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<ClassAsignment> ClassAsignments { get; set; } = new List<ClassAsignment>();
    }
}
