using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.Property(col => col.EnrolledAt).HasDefaultValueSql("GETDATE()");
            builder.Property(col => col.Status).HasConversion<string>();
            builder.HasIndex(col => new { col.StudentId, col.EnrolledYear }).IsUnique();

            builder
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Section)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.SectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.ReasonEnrollment)
                .WithMany(re => re.Enrollments)
                .HasForeignKey(e => e.ReasonEnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
