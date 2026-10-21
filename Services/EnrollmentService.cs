using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;

namespace SchoolHub.Services
{
    public class EnrollmentService(
        AppDbContext context,
        IPaymentService paymentService,
        ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        public async Task<Result> CreateEnrollmentAsync(int idStudent, int sectionId, int reasonEnrollmentId)
        {
            var student = await context.Students.FindAsync(idStudent);
            if (student == null)
            {
                logger.LogWarning("Student with ID {IdStudent} not found.", idStudent);
                return Result.Failure("Student not found.");
            }

            var enrollment = new Enrollment
            {
                StudentId = idStudent,
                SectionId = sectionId,
                ReasonEnrollmentId = reasonEnrollmentId,
            };

            await context.Enrollments.AddAsync(enrollment);
            await context.SaveChangesAsync();

            var paymentResult = await paymentService.CreateEnrollmentObligationAsync(enrollment.Id);
            if (!paymentResult.Succeeded)
            {
                logger.LogError("Enrollment payment obligation could not be created for enrollment {EnrollmentId}: {Error}", enrollment.Id, paymentResult.Error);
                return Result.Failure(paymentResult.Error ?? "The enrollment payment obligation could not be created.");
            }

            logger.LogInformation("Enrollment created for student with ID {IdStudent}.", idStudent);
            return Result.Success();
        }
    }
}