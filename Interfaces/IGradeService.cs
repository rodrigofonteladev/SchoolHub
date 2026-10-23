using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Grade;

namespace SchoolHub.Interfaces
{
    public interface IGradeService
    {
        Task<List<ListGradeViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name, int levelId);
        Task<RegisterGradeViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string name, int levelId);
    }
}
