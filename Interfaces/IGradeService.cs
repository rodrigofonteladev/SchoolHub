using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Grade;

namespace SchoolHub.Interfaces
{
    public interface IGradeService
    {
        Task<List<ListGradeViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name, int levelId);
    }
}