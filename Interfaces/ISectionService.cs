using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Section;

namespace SchoolHub.Interfaces
{
    public interface ISectionService
    {
        Task<List<ListSectionViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name, int gradeId);
    }
}