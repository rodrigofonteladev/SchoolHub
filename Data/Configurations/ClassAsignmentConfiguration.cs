using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class ClassAsignmentConfiguration : IEntityTypeConfiguration<ClassAsignment>
    {
        public void Configure(EntityTypeBuilder<ClassAsignment> builder)
        {
            builder.HasIndex(col => new { col.TeacherId, col.CourseId, col.ScheduleId, col.SectionId, col.AssignedYear }).IsUnique();

            builder
                .HasOne(ca => ca.Teacher)
                .WithMany(t => t.ClassAsignments)
                .HasForeignKey(ca => ca.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ca => ca.Course)
                .WithMany(c => c.ClassAsignments)
                .HasForeignKey(ca => ca.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ca => ca.Schedule)
                .WithMany(sh => sh.ClassAsignments)
                .HasForeignKey(ca => ca.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ca => ca.Section)
                .WithMany(se => se.ClassAsignments)
                .HasForeignKey(ca => ca.SectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
