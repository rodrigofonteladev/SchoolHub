namespace SchoolHub.Models.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateOnly PurchasedAt { get; set; }
        public int TotalPoints { get; set; }
        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
