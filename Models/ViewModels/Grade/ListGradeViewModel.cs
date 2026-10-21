namespace SchoolHub.Models.ViewModels.Grade
{
    public class ListGradeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LevelName { get; set; } = null!;
        public int SectionCount { get; set; }
    }
}