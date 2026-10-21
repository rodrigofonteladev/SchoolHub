using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(col => col.Name).HasMaxLength(50).IsRequired();
            builder.Property(col => col.Description).HasMaxLength(256);
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Product_PricePoints", "PricePoints >= 0");
                t.HasCheckConstraint("CK_Product_Stock", "Stock >= 0");
            });
        }
    }
}
