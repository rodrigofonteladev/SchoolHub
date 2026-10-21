using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(col => col.FullName).HasMaxLength(50).IsRequired();
            builder.Property(col => col.PaternalSurname).HasMaxLength(50).IsRequired();
            builder.Property(col => col.MaternalSurname).HasMaxLength(50).IsRequired();
            builder.Property(col => col.DocumentNumber).HasMaxLength(15).IsRequired();
            builder.Property(col => col.Email).HasMaxLength(35).IsRequired();
            builder.Property(col => col.Address).HasMaxLength(100).IsRequired();

            builder
                .HasOne(t => t.DocumentType)
                .WithMany(dt => dt.Teachers)
                .HasForeignKey(t => t.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
