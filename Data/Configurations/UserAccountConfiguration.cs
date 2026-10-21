using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.Property(col => col.UserCode).HasMaxLength(10).IsRequired();
            builder.HasIndex(col => col.UserCode).IsUnique();
            builder.HasIndex(col => col.StudentId).IsUnique();
            builder.HasIndex(col => col.TeacherId).IsUnique();
            builder.HasIndex(col => col.ApplicationUserId).IsUnique();
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_UserAccount_Student_Teacher", "StudentId IS NULL OR TeacherId IS NULL");
            });

            builder
                .HasOne(ua => ua.Student)
                .WithOne(s => s.UserAccount)
                .HasForeignKey<UserAccount>(ua => ua.StudentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ua => ua.Teacher)
                .WithOne(t => t.UserAccount)
                .HasForeignKey<UserAccount>(ua => ua.TeacherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ua => ua.ApplicationUser)
                .WithOne(au => au.UserAccount)
                .HasForeignKey<UserAccount>(ua => ua.ApplicationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
