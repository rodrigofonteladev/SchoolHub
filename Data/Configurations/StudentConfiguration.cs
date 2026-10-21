using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(col => col.FullName).HasMaxLength(50).IsRequired();
            builder.Property(col => col.PaternalSurname).HasMaxLength(50).IsRequired();
            builder.Property(col => col.MaternalSurname).HasMaxLength(50).IsRequired();
            builder.Property(col => col.DocumentNumber).HasMaxLength(15).IsRequired();
            builder.Property(col => col.Email).HasMaxLength(35).IsRequired();
            builder.Property(col => col.Address).HasMaxLength(100).IsRequired();
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Student_PointsBalance", "PointsBalance >= 0");
            });

            builder
                .HasOne(s => s.DocumentType)
                .WithMany(dt => dt.Students)
                .HasForeignKey(s => s.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
