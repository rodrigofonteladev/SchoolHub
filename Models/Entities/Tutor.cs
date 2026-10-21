using SchoolHub.Common.Entities;
using SchoolHub.Common.Enums;

namespace SchoolHub.Models.Entities
{
    public class Tutor : BaseEntity
    {
        public TutorType Relationship { get; set; }
        public int StudentId { get; set; }
        public int DocumentTypeId { get; set; }

        public Student Student { get; set; } = null!;
        public DocumentType DocumentType { get; set; } = null!;
    }
}
