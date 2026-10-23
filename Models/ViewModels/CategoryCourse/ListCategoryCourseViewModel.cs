namespace SchoolHub.Models.ViewModels.CategoryCourse
{
    public class ListCategoryCourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CourseCount { get; set; }
    }
}
