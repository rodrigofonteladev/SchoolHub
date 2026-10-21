using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
        {
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_PurchaseDetail_QuantityUnits", "QuantityUnits > 0");
                t.HasCheckConstraint("CK_PurchaseDetail_SubTotalPoints", "SubTotalPoints >= 0");
            });

            builder
                .HasOne(pd => pd.Purchase)
                .WithMany(p => p.PurchaseDetails)
                .HasForeignKey(pd => pd.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(pd => pd.Product)
                .WithMany(pr => pr.PurchaseDetails)
                .HasForeignKey(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
