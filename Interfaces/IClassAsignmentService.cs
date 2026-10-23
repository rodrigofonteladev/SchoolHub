using SchoolHub.Common;
using SchoolHub.Models.ViewModels.ClassAsignment;

namespace SchoolHub.Interfaces
{
    public interface IClassAsignmentService
    {
        Task<List<ListClassAsignmentViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterClassAsignmentViewModel model);
        Task<RegisterClassAsignmentViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterClassAsignmentViewModel model);
    }
}
