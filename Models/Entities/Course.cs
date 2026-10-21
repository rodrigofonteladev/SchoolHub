namespace SchoolHub.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int WeeklyHours { get; set; }
        public int CategoryCourseId { get; set; }

        public CategoryCourse CategoryCourse { get; set; } = null!;
        public ICollection<ClassAsignment> ClassAsignments { get; set; } = new List<ClassAsignment>();
    }
}
