using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.Property(col => col.Name).HasMaxLength(1).IsRequired();
            builder.HasIndex(col => new { col.GradeId, col.Name }).IsUnique();
            builder.Property(col => col.LimitStudents).HasDefaultValue(30);
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Section_LimitStudents", "LimitStudents > 0");
            });

            builder
                .HasOne(s => s.Grade)
                .WithMany(g => g.Sections)
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
