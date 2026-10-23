using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.ClassAsignment;

namespace SchoolHub.Services
{
    public class ClassAsignmentService(AppDbContext context) : IClassAsignmentService
    {
        public async Task<List<ListClassAsignmentViewModel>> GetAllAsync() => await context.ClassAsignments.AsNoTracking().OrderByDescending(item => item.AssignedYear).ThenBy(item => item.Course.Name).Select(item => new ListClassAsignmentViewModel
        { Id = item.Id, AssignedYear = item.AssignedYear, CourseName = item.Course.Name, TeacherName = $"{item.Teacher.FullName} {item.Teacher.PaternalSurname}", SectionName = $"{item.Section.Grade.Name} {item.Section.Name}", ScheduleDescription = $"{item.Schedule.DayWeek.Name} {item.Schedule.StartTime}-{item.Schedule.EndTime}", RatingCount = item.Ratings.Count }).ToListAsync();

        public async Task<Result> CreateAsync(RegisterClassAsignmentViewModel model)
        {
            var currentYear = DateTime.Now.Year;
            if (!await context.Teachers.AnyAsync(item => item.Id == model.TeacherId) || !await context.Courses.AnyAsync(item => item.Id == model.CourseId) || !await context.Schedules.AnyAsync(item => item.Id == model.ScheduleId) || !await context.Sections.AnyAsync(item => item.Id == model.SectionId)) return Result.Failure("One or more selected records do not exist.");
            if (await context.ClassAsignments.AnyAsync(item => item.TeacherId == model.TeacherId && item.CourseId == model.CourseId && item.ScheduleId == model.ScheduleId && item.SectionId == model.SectionId && item.AssignedYear == currentYear)) return Result.Failure("That class assignment already exists.");
            await context.ClassAsignments.AddAsync(new ClassAsignment { AssignedYear = currentYear, TeacherId = model.TeacherId!.Value, CourseId = model.CourseId!.Value, ScheduleId = model.ScheduleId!.Value, SectionId = model.SectionId!.Value });
            await context.SaveChangesAsync(); return Result.Success();
        }

        public async Task<RegisterClassAsignmentViewModel?> GetForEditAsync(int id) => await context.ClassAsignments.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterClassAsignmentViewModel { AssignedYear = item.AssignedYear, TeacherId = item.TeacherId, CourseId = item.CourseId, ScheduleId = item.ScheduleId, SectionId = item.SectionId }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterClassAsignmentViewModel model)
        {
            var currentYear = DateTime.Now.Year;
            var entity = await context.ClassAsignments.FindAsync(id);
            if (entity is null) return Result.Failure("The class assignment does not exist.");
            if (!await context.Teachers.AnyAsync(item => item.Id == model.TeacherId) || !await context.Courses.AnyAsync(item => item.Id == model.CourseId) || !await context.Schedules.AnyAsync(item => item.Id == model.ScheduleId) || !await context.Sections.AnyAsync(item => item.Id == model.SectionId)) return Result.Failure("One or more selected records do not exist.");
            if (await context.ClassAsignments.AnyAsync(item => item.Id != id && item.TeacherId == model.TeacherId && item.CourseId == model.CourseId && item.ScheduleId == model.ScheduleId && item.SectionId == model.SectionId && item.AssignedYear == currentYear)) return Result.Failure("That class assignment already exists.");
            entity.AssignedYear = currentYear; entity.TeacherId = model.TeacherId!.Value; entity.CourseId = model.CourseId!.Value; entity.ScheduleId = model.ScheduleId!.Value; entity.SectionId = model.SectionId!.Value; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
