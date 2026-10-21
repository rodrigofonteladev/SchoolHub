using SchoolHub.Common.Enums;

namespace SchoolHub.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "PEN";
        public int? AcademicYear { get; set; }
        public int? InstallmentNumber { get; set; }
        public string? Provider { get; set; }
        public string? ExternalPreferenceId { get; set; }
        public string? CheckoutUrl { get; set; }
        public string? ExternalPaymentId { get; set; }
        public string? ExternalReference { get; set; }
        public string? IdempotencyKey { get; set; }
        public string ConceptKey { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public Enrollment Enrollment { get; set; } = null!;
    }
}