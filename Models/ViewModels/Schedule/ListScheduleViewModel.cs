namespace SchoolHub.Models.ViewModels.Schedule
{
    public class ListScheduleViewModel
    {
        public int Id { get; set; }
        public string DayWeekName { get; set; } = null!;
        public string ShiftName { get; set; } = null!;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int ClassAssignmentCount { get; set; }
    }
}
