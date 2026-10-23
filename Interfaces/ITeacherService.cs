using SchoolHub.Models.ViewModels.Teacher;

namespace SchoolHub.Interfaces
{
    public interface ITeacherService
    {
        Task<List<ListTeacherViewModel>> GetAllAsync();
        Task<SchoolHub.Common.Result> CreateAsync(RegisterTeacherViewModel model);
        Task<RegisterTeacherViewModel?> GetForEditAsync(int id);
        Task<SchoolHub.Common.Result> UpdateAsync(int id, RegisterTeacherViewModel model);
    }
}
