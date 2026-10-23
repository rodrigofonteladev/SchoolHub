using SchoolHub.Common.Enums;

namespace SchoolHub.Models.ViewModels.Tutor
{
    public class ListTutorViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PaternalSurname { get; set; } = null!;
        public string MaternalSurname { get; set; } = null!;
        public string DocumentTypeName { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string StudentName { get; set; } = null!;
        public TutorType Relationship { get; set; }
        public string Email { get; set; } = null!;
    }
}
