namespace SchoolHub.Models.ViewModels.Product
{
    public class ListProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int PricePoints { get; set; }
        public int Stock { get; set; }
        public int PurchaseCount { get; set; }
    }
}
