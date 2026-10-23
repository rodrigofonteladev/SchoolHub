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

        public async Task<RegisterLevelViewModel?> GetForEditAsync(int id) => await context.Levels.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterLevelViewModel { Name = item.Name }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, string name)
        {
            var entity = await context.Levels.FindAsync(id);
            if (entity is null) return Result.Failure("The level does not exist.");
            if (await context.Levels.AnyAsync(item => item.Id != id && item.Name == name)) return Result.Failure("A level with that name already exists.");
            entity.Name = name; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
