using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Level;

namespace SchoolHub.Interfaces
{
    public interface ILevelService
    {
        Task<List<ListLevelViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name);
        Task<RegisterLevelViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string name);
    }
}
