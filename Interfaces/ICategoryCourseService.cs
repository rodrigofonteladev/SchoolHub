using SchoolHub.Common;
using SchoolHub.Models.ViewModels.CategoryCourse;

namespace SchoolHub.Interfaces
{
    public interface ICategoryCourseService
    {
        Task<List<ListCategoryCourseViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterCategoryCourseViewModel model);
        Task<RegisterCategoryCourseViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterCategoryCourseViewModel model);
    }
}
