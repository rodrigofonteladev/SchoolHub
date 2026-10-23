namespace SchoolHub.Models.ViewModels.Course
{
    public class ListCourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int WeeklyHours { get; set; }
        public string CategoryName { get; set; } = null!;
        public int ClassAssignmentCount { get; set; }
    }
}
