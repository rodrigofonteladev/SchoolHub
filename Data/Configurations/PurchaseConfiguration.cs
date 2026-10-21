using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.Property(col => col.PurchasedAt).HasDefaultValueSql("GETDATE()");
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Purchase_TotalPoints", "TotalPoints >= 0");
            });

            builder
                .HasOne(p => p.Student)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
