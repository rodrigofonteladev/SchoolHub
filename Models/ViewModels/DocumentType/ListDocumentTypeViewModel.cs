namespace SchoolHub.Models.ViewModels.DocumentType
{
    public class ListDocumentTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public int TutorCount { get; set; }
    }
}