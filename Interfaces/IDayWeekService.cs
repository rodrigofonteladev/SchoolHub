using SchoolHub.Common;
using SchoolHub.Models.ViewModels.DayWeek;

namespace SchoolHub.Interfaces
{
    public interface IDayWeekService
    {
        Task<List<ListDayWeekViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name);
        Task<RegisterDayWeekViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string name);
    }
}
