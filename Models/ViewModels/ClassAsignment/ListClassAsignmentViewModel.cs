namespace SchoolHub.Models.ViewModels.ClassAsignment
{
    public class ListClassAsignmentViewModel
    {
        public int Id { get; set; }
        public int AssignedYear { get; set; }
        public string TeacherName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string SectionName { get; set; } = null!;
        public string ScheduleDescription { get; set; } = null!;
        public int RatingCount { get; set; }
    }
}
