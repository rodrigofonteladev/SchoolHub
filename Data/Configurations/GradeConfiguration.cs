using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.Property(col => col.Name).HasMaxLength(5).IsRequired();
            builder.HasIndex(col => new { col.LevelId, col.Name }).IsUnique();

            builder
                .HasOne(g => g.Level)
                .WithMany(l => l.Grades)
                .HasForeignKey(g => g.LevelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
