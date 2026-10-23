using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Student;

namespace SchoolHub.Interfaces
{
    public interface IStudentService
    {
        Task<List<ListStudentViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterStudentViewModel model);
        Task<EditStudentViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, EditStudentViewModel model);
    }
}
