using SchoolHub.Common.Entities;

namespace SchoolHub.Models.Entities
{
    public class Teacher : BaseEntity
    {
        public int DocumentTypeId { get; set; }

        public DocumentType DocumentType { get; set; } = null!;
        public UserAccount? UserAccount { get; set; }
        public ICollection<ClassAsignment> ClassAsignments { get; set; } = new List<ClassAsignment>();
    }
}
