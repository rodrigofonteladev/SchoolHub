using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.Property(col => col.Exam1).HasPrecision(3, 1);
            builder.Property(col => col.Exam2).HasPrecision(3, 1);
            builder.Property(col => col.Exam3).HasPrecision(3, 1);
            builder.HasIndex(col => new { col.EnrollmentId, col.ClassAsignmentId }).IsUnique();
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Rating_Exam1", "Exam1 BETWEEN 0 AND 20");
                t.HasCheckConstraint("CK_Rating_Exam2", "Exam2 BETWEEN 0 AND 20");
                t.HasCheckConstraint("CK_Rating_Exam3", "Exam3 BETWEEN 0 AND 20");
            });

            builder
                .HasOne(r => r.Enrollment)
                .WithMany(e => e.Ratings)
                .HasForeignKey(r => r.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(r => r.ClassAsignment)
                .WithMany(ca => ca.Ratings)
                .HasForeignKey(r => r.ClassAsignmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
