using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Level;

namespace SchoolHub.Services
{
    public class LevelService(AppDbContext context) : ILevelService
    {
        public async Task<List<ListLevelViewModel>> GetAllAsync()
        {
            return await context.Levels
                .AsNoTracking()
                .OrderBy(level => level.Name)
                .Select(level => new ListLevelViewModel
                {
                    Id = level.Id,
                    Name = level.Name,
                    GradeCount = level.Grades.Count
                })
                .ToListAsync();
        }

        public async Task<Result> CreateAsync(string name)
        {
            if (await context.Levels.AnyAsync(level => level.Name == name))
            {
                return Result.Failure("A level with that name already exists.");
            }

            await context.Levels.AddAsync(new Level { Name = name });
            await context.SaveChangesAsync();

            return Result.Success();
        }
    }
}