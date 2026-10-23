using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Enrollment;

namespace SchoolHub.Services
{
    public class EnrollmentService(
        AppDbContext context,
        IPaymentService paymentService,
        ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        public async Task<List<ListEnrollmentViewModel>> GetAllAsync() => await context.Enrollments.AsNoTracking()
            .OrderByDescending(item => item.EnrolledYear).ThenBy(item => item.Student.PaternalSurname)
            .Select(item => new ListEnrollmentViewModel
            {
                Id = item.Id,
                StudentName = $"{item.Student.FullName} {item.Student.PaternalSurname} {item.Student.MaternalSurname}",
                SectionName = item.Section.Name,
                GradeName = item.Section.Grade.Name,
                Reason = item.ReasonEnrollment.Description,
                EnrolledYear = item.EnrolledYear,
                EnrolledAt = item.EnrolledAt,
                Status = item.Status
            }).ToListAsync();

        public async Task<Result> CreateAsync(RegisterEnrollmentViewModel model)
        {
            if (!model.StudentId.HasValue || !model.SectionId.HasValue || !model.ReasonEnrollmentId.HasValue)
                return Result.Failure("The enrollment data is incomplete.");
            if (!await context.Students.AnyAsync(item => item.Id == model.StudentId.Value) || !await context.Sections.AnyAsync(item => item.Id == model.SectionId.Value) || !await context.ReasonEnrollments.AnyAsync(item => item.Id == model.ReasonEnrollmentId.Value))
                return Result.Failure("One or more selected records do not exist.");
            if (await context.Enrollments.AnyAsync(item => item.StudentId == model.StudentId.Value && item.EnrolledYear == model.EnrolledYear))
                return Result.Failure("The student already has an enrollment for that year.");
            var section = await context.Sections.Include(item => item.Enrollments).FirstAsync(item => item.Id == model.SectionId.Value);
            if (section.Enrollments.Count(item => item.Status != Common.Enums.EnrollmentStatus.Cancelled) >= section.LimitStudents)
                return Result.Failure("The selected section has no available places.");
            var enrollment = new Enrollment { StudentId = model.StudentId.Value, SectionId = model.SectionId.Value, ReasonEnrollmentId = model.ReasonEnrollmentId.Value, EnrolledYear = model.EnrolledYear };
            await context.Enrollments.AddAsync(enrollment);
            await context.SaveChangesAsync();
            var paymentResult = await paymentService.CreateEnrollmentObligationAsync(enrollment.Id);
            return paymentResult.Succeeded ? Result.Success() : Result.Failure(paymentResult.Error ?? "The enrollment payment obligation could not be created.");
        }

        public async Task<EditEnrollmentViewModel?> GetForEditAsync(int id) => await context.Enrollments.AsNoTracking().Where(item => item.Id == id).Select(item => new EditEnrollmentViewModel { StudentName = $"{item.Student.FullName} {item.Student.PaternalSurname} {item.Student.MaternalSurname}", EnrolledYear = item.EnrolledYear, SectionId = item.SectionId, ReasonEnrollmentId = item.ReasonEnrollmentId, Status = item.Status }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, EditEnrollmentViewModel model)
        {
            var entity = await context.Enrollments.FindAsync(id);
            if (entity is null) return Result.Failure("The enrollment does not exist.");
            if (!await context.Sections.AnyAsync(item => item.Id == model.SectionId) || !await context.ReasonEnrollments.AnyAsync(item => item.Id == model.ReasonEnrollmentId)) return Result.Failure("The selected section or reason does not exist.");
            entity.SectionId = model.SectionId!.Value; entity.ReasonEnrollmentId = model.ReasonEnrollmentId!.Value; entity.Status = model.Status; await context.SaveChangesAsync(); return Result.Success();
        }

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
