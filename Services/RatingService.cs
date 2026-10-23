using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Rating;

namespace SchoolHub.Services
{
    public class RatingService(AppDbContext context) : IRatingService
    {
        public async Task<List<ListRatingViewModel>> GetAllAsync() => await context.Ratings.AsNoTracking().OrderByDescending(item => item.Enrollment.EnrolledYear).Select(item => new ListRatingViewModel
        { Id = item.Id, StudentName = $"{item.Enrollment.Student.FullName} {item.Enrollment.Student.PaternalSurname}", CourseName = item.ClassAsignment.Course.Name, TeacherName = item.ClassAsignment.Teacher.FullName, EnrolledYear = item.Enrollment.EnrolledYear, Exam1 = item.Exam1, Exam2 = item.Exam2, Exam3 = item.Exam3 }).ToListAsync();

        public async Task<Result> CreateAsync(RegisterRatingViewModel model)
        {
            var enrollment = await context.Enrollments.FirstOrDefaultAsync(item => item.Id == model.EnrollmentId);
            var assignment = await context.ClassAsignments.FirstOrDefaultAsync(item => item.Id == model.ClassAsignmentId);
            if (enrollment is null || assignment is null) return Result.Failure("The enrollment or class assignment does not exist.");
            if (enrollment.SectionId != assignment.SectionId || enrollment.EnrolledYear != assignment.AssignedYear) return Result.Failure("The enrollment and class assignment do not belong to the same section and year.");
            if (await context.Ratings.AnyAsync(item => item.EnrollmentId == model.EnrollmentId && item.ClassAsignmentId == model.ClassAsignmentId)) return Result.Failure("A rating already exists for that enrollment and class assignment.");
            await context.Ratings.AddAsync(new Rating { EnrollmentId = model.EnrollmentId!.Value, ClassAsignmentId = model.ClassAsignmentId!.Value, Exam1 = model.Exam1, Exam2 = model.Exam2, Exam3 = model.Exam3 });
            await context.SaveChangesAsync(); return Result.Success();
        }

        public async Task<RegisterRatingViewModel?> GetForEditAsync(int id) => await context.Ratings.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterRatingViewModel { EnrollmentId = item.EnrollmentId, ClassAsignmentId = item.ClassAsignmentId, Exam1 = item.Exam1, Exam2 = item.Exam2, Exam3 = item.Exam3 }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterRatingViewModel model)
        {
            var entity = await context.Ratings.FindAsync(id);
            if (entity is null) return Result.Failure("The rating does not exist.");
            var enrollment = await context.Enrollments.FindAsync(model.EnrollmentId);
            var assignment = await context.ClassAsignments.FindAsync(model.ClassAsignmentId);
            if (enrollment is null || assignment is null || enrollment.SectionId != assignment.SectionId || enrollment.EnrolledYear != assignment.AssignedYear) return Result.Failure("The enrollment and class assignment are not compatible.");
            if (await context.Ratings.AnyAsync(item => item.Id != id && item.EnrollmentId == model.EnrollmentId && item.ClassAsignmentId == model.ClassAsignmentId)) return Result.Failure("A rating already exists for that enrollment and class assignment.");
            entity.EnrollmentId = model.EnrollmentId!.Value; entity.ClassAsignmentId = model.ClassAsignmentId!.Value; entity.Exam1 = model.Exam1; entity.Exam2 = model.Exam2; entity.Exam3 = model.Exam3; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
