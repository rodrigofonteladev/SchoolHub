namespace SchoolHub.Models.ViewModels.Teacher
{
    public class ListTeacherViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PaternalSurname { get; set; } = null!;
        public string MaternalSurname { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string DocumentTypeName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public int ClassAssignmentCount { get; set; }
    }
}