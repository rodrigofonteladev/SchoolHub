using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Shift;

namespace SchoolHub.Services
{
    public class ShiftService(AppDbContext context) : IShiftService
    {
        public async Task<List<ListShiftViewModel>> GetAllAsync() => await context.Shifts.AsNoTracking().OrderBy(item => item.Name).Select(item => new ListShiftViewModel
        { Id = item.Id, Name = item.Name, ScheduleCount = item.Schedules.Count }).ToListAsync();

        public async Task<Result> CreateAsync(string name)
        {
            if (await context.Shifts.AnyAsync(item => item.Name == name)) return Result.Failure("A shift with that name already exists.");
            await context.Shifts.AddAsync(new Shift { Name = name }); await context.SaveChangesAsync(); return Result.Success();
        }

        public async Task<RegisterShiftViewModel?> GetForEditAsync(int id) => await context.Shifts.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterShiftViewModel { Name = item.Name }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, string name)
        {
            var entity = await context.Shifts.FindAsync(id);
            if (entity is null) return Result.Failure("The shift does not exist.");
            if (await context.Shifts.AnyAsync(item => item.Id != id && item.Name == name)) return Result.Failure("A shift with that name already exists.");
            entity.Name = name; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
