using SchoolHub.Common.Entities;

namespace SchoolHub.Models.Entities
{
    public class Student : BaseEntity
    {
        public DateOnly Birthday { get; set; }
        public int PointsBalance { get; set; }
        public int DocumentTypeId { get; set; }

        public DocumentType DocumentType { get; set; } = null!;
        public UserAccount? UserAccount { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
    }
}
