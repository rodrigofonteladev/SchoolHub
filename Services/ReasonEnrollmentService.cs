using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.ReasonEnrollment;

namespace SchoolHub.Services
{
    public class ReasonEnrollmentService(AppDbContext context) : IReasonEnrollmentService
    {
        public async Task<List<ListReasonEnrollmentViewModel>> GetAllAsync()
        {
            return await context.ReasonEnrollments
                .AsNoTracking()
                .OrderBy(reason => reason.Description)
                .Select(reason => new ListReasonEnrollmentViewModel
                {
                    Id = reason.Id,
                    Description = reason.Description,
                    EnrollmentCount = reason.Enrollments.Count
                })
                .ToListAsync();
        }

        public async Task<Result> CreateAsync(string description)
        {
            if (await context.ReasonEnrollments.AnyAsync(reason => reason.Description == description))
            {
                return Result.Failure("An enrollment reason with that description already exists.");
            }

            await context.ReasonEnrollments.AddAsync(new ReasonEnrollment
            {
                Description = description
            });
            await context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<RegisterReasonEnrollmentViewModel?> GetForEditAsync(int id) => await context.ReasonEnrollments.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterReasonEnrollmentViewModel { Description = item.Description }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, string description)
        {
            var entity = await context.ReasonEnrollments.FindAsync(id);
            if (entity is null) return Result.Failure("The enrollment reason does not exist.");
            if (await context.ReasonEnrollments.AnyAsync(item => item.Id != id && item.Description == description)) return Result.Failure("That enrollment reason already exists.");
            entity.Description = description; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
