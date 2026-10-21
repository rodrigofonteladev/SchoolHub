using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class PointConversionRateConfiguration : IEntityTypeConfiguration<PointConversionRate>
    {
        public void Configure(EntityTypeBuilder<PointConversionRate> builder)
        {
            builder.Property(col => col.AmountMoney).HasPrecision(7, 2);
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_PointConversionRate_AmountMoney", "AmountMoney > 0");
                t.HasCheckConstraint("CK_PointConversionRate_QuantityPoints", "QuantityPoints > 0");
                t.HasCheckConstraint("CK_PointConversionRate_AdditionalBonus", "AdditionalBonus >= 0");
            });
        }
    }
}
