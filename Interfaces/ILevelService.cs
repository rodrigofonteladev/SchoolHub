using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Level;

namespace SchoolHub.Interfaces
{
    public interface ILevelService
    {
        Task<List<ListLevelViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name);
    }
}