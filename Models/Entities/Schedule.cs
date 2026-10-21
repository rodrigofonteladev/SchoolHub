namespace SchoolHub.Models.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int ShiftId { get; set; }
        public int DayWeekId { get; set; }

        public Shift Shift { get; set; } = null!;
        public DayWeek DayWeek { get; set; } = null!;
        public ICollection<ClassAsignment> ClassAsignments { get; set; } = new List<ClassAsignment>();
    }
}
