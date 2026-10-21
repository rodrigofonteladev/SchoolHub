namespace SchoolHub.Models.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }
        public required string UserCode { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public required string ApplicationUserId { get; set; }

        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
