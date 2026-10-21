using SchoolHub.Common.Enums;

namespace SchoolHub.Models.ViewModels.Payment
{
    public class ListPaymentViewModel
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public string StudentName { get; set; } = null!;
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public int? AcademicYear { get; set; }
        public int? InstallmentNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}