using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Section;

namespace SchoolHub.Interfaces
{
    public interface ISectionService
    {
        Task<List<ListSectionViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name, int gradeId);
        Task<RegisterSectionViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string name, int gradeId);
    }
}
