using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class ReasonEnrollmentConfiguration : IEntityTypeConfiguration<ReasonEnrollment>
    {
        public void Configure(EntityTypeBuilder<ReasonEnrollment> builder)
        {
            builder.Property(col => col.Description).HasMaxLength(256).IsRequired();
        }
    }
}
