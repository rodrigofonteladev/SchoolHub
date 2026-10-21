using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class CategoryCourseConfiguration : IEntityTypeConfiguration<CategoryCourse>
    {
        public void Configure(EntityTypeBuilder<CategoryCourse> builder)
        {
            builder.Property(col => col.Name).HasMaxLength(50).IsRequired();
            builder.Property(col => col.Description).HasMaxLength(256);
        }
    }
}
