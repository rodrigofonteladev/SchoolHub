namespace SchoolHub.Common.Settings
{
    public class PaymentSettings
    {
        public const string SectionName = "PaymentSettings";

        public decimal EnrollmentAmount { get; set; } = 410m;
        public decimal PensionAmount { get; set; } = 400m;
        public int PensionInstallments { get; set; } = 10;
        public string Currency { get; set; } = "PEN";
    }
}