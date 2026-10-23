using SchoolHub.Common;
using SchoolHub.Models.ViewModels.ReasonEnrollment;

namespace SchoolHub.Interfaces
{
    public interface IReasonEnrollmentService
    {
        Task<List<ListReasonEnrollmentViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string description);
        Task<RegisterReasonEnrollmentViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string description);
    }
}
