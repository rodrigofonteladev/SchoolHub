using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Enrollment;

namespace SchoolHub.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Result> CreateEnrollmentAsync(int idStudent, int sectionId, int reasonEnrollmentId);
        Task<List<ListEnrollmentViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterEnrollmentViewModel model);
        Task<EditEnrollmentViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, EditEnrollmentViewModel model);
    }
}
