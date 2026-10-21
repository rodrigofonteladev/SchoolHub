namespace SchoolHub.Models.Entities
{
    public class PurchaseDetail
    {
        public int Id { get; set; }
        public int QuantityUnits { get; set; }
        public int SubTotalPoints { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }

        public Purchase Purchase { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
