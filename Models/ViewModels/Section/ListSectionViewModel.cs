namespace SchoolHub.Models.ViewModels.Section
{
    public class ListSectionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string GradeName { get; set; } = null!;
        public string LevelName { get; set; } = null!;
        public int LimitQuantity { get; set; }
        public int EnrollmentCount { get; set; }
    }
}