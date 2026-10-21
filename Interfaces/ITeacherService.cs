using SchoolHub.Models.ViewModels.Teacher;

namespace SchoolHub.Interfaces
{
    public interface ITeacherService
    {
        Task<List<ListTeacherViewModel>> GetAllAsync();
    }
}