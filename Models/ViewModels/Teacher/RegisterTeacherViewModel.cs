using SchoolHub.Models.ViewModels.Shared;

namespace SchoolHub.Models.ViewModels.Teacher
{
    public class RegisterTeacherViewModel : RegisterPersonViewModel
    {
        public int DocumentTypeId { get; set; }
    }
}
