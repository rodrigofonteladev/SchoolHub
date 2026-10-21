namespace SchoolHub.Models.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int LevelId { get; set; }

        public Level Level { get; set; } = null!;
        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
