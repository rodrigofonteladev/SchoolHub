namespace SchoolHub.Models.Entities
{
    public class Level
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}
