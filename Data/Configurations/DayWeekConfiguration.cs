using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class DayWeekConfiguration : IEntityTypeConfiguration<DayWeek>
    {
        public void Configure(EntityTypeBuilder<DayWeek> builder)
        {
            builder.Property(col => col.Name).HasMaxLength(10).IsRequired();
        }
    }
}
