namespace SchoolHub.Models.ViewModels.Payment
{
    public class PaymentCheckoutViewModel
    {
        public int PaymentId { get; set; }
        public string CheckoutUrl { get; set; } = null!;
    }
}