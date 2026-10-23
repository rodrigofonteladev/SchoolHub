using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.CategoryCourse;

namespace SchoolHub.Services
{
    public class CategoryCourseService(AppDbContext context) : ICategoryCourseService
    {
        public async Task<List<ListCategoryCourseViewModel>> GetAllAsync()
        {
            return await context.CategoryCourses.AsNoTracking()
                .OrderBy(category => category.Name)
                .Select(category => new ListCategoryCourseViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CourseCount = category.Courses.Count
                }).ToListAsync();
        }

        public async Task<Result> CreateAsync(RegisterCategoryCourseViewModel model)
        {
            if (await context.CategoryCourses.AnyAsync(category => category.Name == model.Name))
                return Result.Failure("A course category with that name already exists.");

            await context.CategoryCourses.AddAsync(new CategoryCourse { Name = model.Name, Description = model.Description });
            await context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<RegisterCategoryCourseViewModel?> GetForEditAsync(int id) => await context.CategoryCourses.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterCategoryCourseViewModel { Name = item.Name, Description = item.Description }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterCategoryCourseViewModel model)
        {
            var entity = await context.CategoryCourses.FindAsync(id);
            if (entity is null) return Result.Failure("The course category does not exist.");
            if (await context.CategoryCourses.AnyAsync(item => item.Id != id && item.Name == model.Name)) return Result.Failure("A course category with that name already exists.");
            entity.Name = model.Name; entity.Description = model.Description; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
