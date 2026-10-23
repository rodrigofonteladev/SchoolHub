using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Tutor;

namespace SchoolHub.Interfaces
{
    public interface ITutorService
    {
        Task<List<ListTutorViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterTutorViewModel model);
        Task<RegisterTutorViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterTutorViewModel model);
    }
}
