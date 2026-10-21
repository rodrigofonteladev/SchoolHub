using SchoolHub.Common;

namespace SchoolHub.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Result> CreateEnrollmentAsync(int idStudent, int sectionId, int reasonEnrollmentId);
    }
}