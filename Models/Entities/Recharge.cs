namespace SchoolHub.Models.Entities
{
    public class Recharge
    {
        public int Id { get; set; }
        public DateOnly RechargedAt { get; set; }
        public decimal Amount { get; set; }
        public int QuantityPointsAwarded { get; set; }
        public int StudentId { get; set; }
        public int PointConversionRateId { get; set; }

        public Student Student { get; set; } = null!;
        public PointConversionRate PointConversionRate { get; set; } = null!;
    }
}
