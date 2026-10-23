using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Shift;

namespace SchoolHub.Interfaces
{
    public interface IShiftService
    {
        Task<List<ListShiftViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name);
        Task<RegisterShiftViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string name);
    }
}
