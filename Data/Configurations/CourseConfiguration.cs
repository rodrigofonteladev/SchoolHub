using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.Property(col => col.Name).HasMaxLength(50).IsRequired();
            builder.Property(col => col.Description).HasMaxLength(256);

            builder
                .HasOne(co => co.CategoryCourse)
                .WithMany(ca => ca.Courses)
                .HasForeignKey(co => co.CategoryCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
