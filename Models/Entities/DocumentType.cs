namespace SchoolHub.Models.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
    }
}
