using Microsoft.EntityFrameworkCore;
using SchoolHub.Common;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Schedule;

namespace SchoolHub.Services
{
    public class ScheduleService(AppDbContext context) : IScheduleService
    {
        public async Task<List<ListScheduleViewModel>> GetAllAsync() => await context.Schedules.AsNoTracking().OrderBy(item => item.DayWeekId).ThenBy(item => item.StartTime).Select(item => new ListScheduleViewModel
        { Id = item.Id, DayWeekName = item.DayWeek.Name, ShiftName = item.Shift.Name, StartTime = item.StartTime, EndTime = item.EndTime, ClassAssignmentCount = item.ClassAsignments.Count }).ToListAsync();

        public async Task<Result> CreateAsync(RegisterScheduleViewModel model)
        {
            if (!model.StartTime.HasValue || !model.EndTime.HasValue || model.StartTime >= model.EndTime) return Result.Failure("The start time must be before the end time.");
            if (!await context.Shifts.AnyAsync(item => item.Id == model.ShiftId) || !await context.DayWeeks.AnyAsync(item => item.Id == model.DayWeekId)) return Result.Failure("The selected shift or day does not exist.");
            if (await context.Schedules.AnyAsync(item => item.ShiftId == model.ShiftId && item.DayWeekId == model.DayWeekId && item.StartTime == model.StartTime && item.EndTime == model.EndTime)) return Result.Failure("That schedule already exists.");
            await context.Schedules.AddAsync(new Schedule { StartTime = model.StartTime.Value, EndTime = model.EndTime.Value, ShiftId = model.ShiftId!.Value, DayWeekId = model.DayWeekId!.Value });
            await context.SaveChangesAsync(); return Result.Success();
        }

        public async Task<RegisterScheduleViewModel?> GetForEditAsync(int id) => await context.Schedules.AsNoTracking().Where(item => item.Id == id).Select(item => new RegisterScheduleViewModel { StartTime = item.StartTime, EndTime = item.EndTime, ShiftId = item.ShiftId, DayWeekId = item.DayWeekId }).FirstOrDefaultAsync();

        public async Task<Result> UpdateAsync(int id, RegisterScheduleViewModel model)
        {
            if (!model.StartTime.HasValue || !model.EndTime.HasValue || model.StartTime >= model.EndTime) return Result.Failure("The start time must be before the end time.");
            var entity = await context.Schedules.FindAsync(id);
            if (entity is null) return Result.Failure("The schedule does not exist.");
            if (!await context.Shifts.AnyAsync(item => item.Id == model.ShiftId) || !await context.DayWeeks.AnyAsync(item => item.Id == model.DayWeekId)) return Result.Failure("The selected shift or day does not exist.");
            if (await context.Schedules.AnyAsync(item => item.Id != id && item.ShiftId == model.ShiftId && item.DayWeekId == model.DayWeekId && item.StartTime == model.StartTime && item.EndTime == model.EndTime)) return Result.Failure("That schedule already exists.");
            entity.StartTime = model.StartTime.Value; entity.EndTime = model.EndTime.Value; entity.ShiftId = model.ShiftId!.Value; entity.DayWeekId = model.DayWeekId!.Value; await context.SaveChangesAsync(); return Result.Success();
        }
    }
}
