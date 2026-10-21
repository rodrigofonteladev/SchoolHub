namespace SchoolHub.Models.ViewModels.Payment
{
    public class PaymentResultViewModel
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? ExternalReference { get; set; }
    }
}