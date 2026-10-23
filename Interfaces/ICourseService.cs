using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Course;

namespace SchoolHub.Interfaces
{
    public interface ICourseService
    {
        Task<List<ListCourseViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterCourseViewModel model);
        Task<RegisterCourseViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterCourseViewModel model);
    }
}
