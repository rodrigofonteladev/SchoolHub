namespace SchoolHub.Models.Entities
{
    public class ClassAsignment
    {
        public int Id { get; set; }
        public int AssignedYear { get; set; }
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public int ScheduleId { get; set; }
        public int SectionId { get; set; }

        public Teacher Teacher { get; set; } = null!;
        public Course Course { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;
        public Section Section { get; set; } = null!;
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
