namespace SchoolHub.Common.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string PaternalSurname { get; set; }
        public required string MaternalSurname { get; set; }
        public required string DocumentNumber { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
