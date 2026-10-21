namespace SchoolHub.Models.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
