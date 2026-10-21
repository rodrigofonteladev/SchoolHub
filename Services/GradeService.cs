using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Grade;

namespace SchoolHub.Services
{
    public class GradeService(AppDbContext context) : IGradeService
    {
        public async Task<List<ListGradeViewModel>> GetAllAsync()
        {
            return await context.Grades
                .AsNoTracking()
                .OrderBy(grade => grade.Level.Name)
                .ThenBy(grade => grade.Name)
                .Select(grade => new ListGradeViewModel
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    LevelName = grade.Level.Name,
                    SectionCount = grade.Sections.Count
                })
                .ToListAsync();
        }

        public async Task<Result> CreateAsync(string name, int levelId)
        {
            if (!await context.Levels.AnyAsync(level => level.Id == levelId))
            {
                return Result.Failure("The selected level does not exist.");
            }

            if (await context.Grades.AnyAsync(grade => grade.LevelId == levelId && grade.Name == name))
            {
                return Result.Failure("A grade with that name already exists in the selected level.");
            }

            await context.Grades.AddAsync(new Grade { Name = name, LevelId = levelId });
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}