using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Course;

namespace SchoolHub.Services
{
    public class CourseService(AppDbContext context) : ICourseService
    {
        public async Task<List<ListCourseViewModel>> GetAllAsync()
        {
            return await context.Courses.AsNoTracking()
                .OrderBy(course => course.CategoryCourse.Name).ThenBy(course => course.Name)
                .Select(course => new ListCourseViewModel
                {
                    Id = course.Id, Name = course.Name, Description = course.Description,
                    WeeklyHours = course.WeeklyHours, CategoryName = course.CategoryCourse.Name,
                    ClassAssignmentCount = course.ClassAsignments.Count
                }).ToListAsync();
        }

        public async Task<Result> CreateAsync(RegisterCourseViewModel model)
        {
            if (!model.CategoryCourseId.HasValue || !await context.CategoryCourses.AnyAsync(category => category.Id == model.CategoryCourseId.Value))
                return Result.Failure("The selected course category does not exist.");

            if (await context.Courses.AnyAsync(course => course.Name == model.Name))
                return Result.Failure("A course with that name already exists.");

            await context.Courses.AddAsync(new Course
            {
                Name = model.Name, Description = model.Description,
                WeeklyHours = model.WeeklyHours, CategoryCourseId = model.CategoryCourseId!.Value
            });
            await context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<RegisterCourseViewModel?> GetForEditAsync(int id) => await context.Courses.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterCourseViewModel { Name = item.Name, Description = item.Description, WeeklyHours = item.WeeklyHours, CategoryCourseId = item.CategoryCourseId }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterCourseViewModel model)
        {
            var entity = await context.Courses.FindAsync(id);
            if (entity is null) return Result.Failure("The course does not exist.");
            if (!await context.CategoryCourses.AnyAsync(item => item.Id == model.CategoryCourseId)) return Result.Failure("The selected course category does not exist.");
            if (await context.Courses.AnyAsync(item => item.Id != id && item.Name == model.Name)) return Result.Failure("A course with that name already exists.");
            entity.Name = model.Name; entity.Description = model.Description; entity.WeeklyHours = model.WeeklyHours; entity.CategoryCourseId = model.CategoryCourseId!.Value; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
