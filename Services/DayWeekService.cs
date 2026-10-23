using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.DayWeek;

namespace SchoolHub.Services
{
    public class DayWeekService(AppDbContext context) : IDayWeekService
    {
        public async Task<List<ListDayWeekViewModel>> GetAllAsync() => await context.DayWeeks
            .AsNoTracking()
            .OrderBy(day => day.Id)
            .Select(day => new ListDayWeekViewModel
            {
                Id = day.Id,
                Name = day.Name,
                ScheduleCount = day.Schedules.Count
            })
            .ToListAsync();

        public async Task<Result> CreateAsync(string name)
        {
            if (await context.DayWeeks.AnyAsync(day => day.Name == name))
                return Result.Failure("A day with that name already exists.");

            await context.DayWeeks.AddAsync(new DayWeek { Name = name });
            await context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<RegisterDayWeekViewModel?> GetForEditAsync(int id) => await context.DayWeeks
            .AsNoTracking()
            .Where(day => day.Id == id)
            .Select(day => new RegisterDayWeekViewModel { Name = day.Name })
            .FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, string name)
        {
            var day = await context.DayWeeks.FindAsync(id);
            if (day is null) return Result.Failure("The day does not exist.");
            if (await context.DayWeeks.AnyAsync(item => item.Id != id && item.Name == name))
                return Result.Failure("A day with that name already exists.");

            day.Name = name;
            await context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
