using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class RechargeConfiguration : IEntityTypeConfiguration<Recharge>
    {
        public void Configure(EntityTypeBuilder<Recharge> builder)
        {
            builder.Property(col => col.RechargedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(col => col.Amount).HasPrecision(7, 2);
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Recharge_Amount", "Amount > 0");
                t.HasCheckConstraint("CK_Recharge_QuantityPointsAwarded", "QuantityPointsAwarded > 0");
            });

            builder
                .HasOne(r => r.Student)
                .WithMany(s => s.Recharges)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(r => r.PointConversionRate)
                .WithMany(pcr => pcr.Recharges)
                .HasForeignKey(r => r.PointConversionRateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
