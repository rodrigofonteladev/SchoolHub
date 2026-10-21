namespace SchoolHub.Models.Entities
{
    public class PointConversionRate
    {
        public int Id { get; set; }
        public decimal AmountMoney { get; set; }
        public int QuantityPoints { get; set; }
        public int AdditionalBonus { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
    }
}
