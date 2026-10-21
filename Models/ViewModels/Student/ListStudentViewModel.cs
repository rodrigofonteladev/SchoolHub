namespace SchoolHub.Models.ViewModels.Student
{
    public class ListStudentViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PaternalSurname { get; set; } = null!;
        public string MaternalSurname { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string DocumentTypeName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly Birthday { get; set; }
        public int PointsBalance { get; set; }
        public bool IsActive { get; set; }
        public int TutorCount { get; set; }
    }
}