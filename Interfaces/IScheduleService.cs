using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Schedule;

namespace SchoolHub.Interfaces
{
    public interface IScheduleService
    {
        Task<List<ListScheduleViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterScheduleViewModel model);
        Task<RegisterScheduleViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterScheduleViewModel model);
    }
}
