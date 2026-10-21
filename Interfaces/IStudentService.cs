using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Student;

namespace SchoolHub.Interfaces
{
    public interface IStudentService
    {
        Task<List<ListStudentViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterStudentViewModel model);
    }
}