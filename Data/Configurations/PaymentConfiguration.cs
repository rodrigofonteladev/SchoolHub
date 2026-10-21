using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Models.Entities;

namespace SchoolHub.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(payment => payment.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(payment => payment.Currency)
                .HasMaxLength(3)
                .IsRequired()
                .HasDefaultValue("PEN");

            builder.Property(payment => payment.Provider).HasMaxLength(50);
            builder.Property(payment => payment.ExternalPreferenceId).HasMaxLength(150);
            builder.Property(payment => payment.CheckoutUrl).HasMaxLength(500);
            builder.Property(payment => payment.ExternalPaymentId).HasMaxLength(150);
            builder.Property(payment => payment.ExternalReference).HasMaxLength(150);
            builder.Property(payment => payment.IdempotencyKey).HasMaxLength(100);
            builder.Property(payment => payment.ConceptKey).HasMaxLength(150).IsRequired();

            builder.HasOne(payment => payment.Enrollment)
                .WithMany(enrollment => enrollment.Payments)
                .HasForeignKey(payment => payment.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(payment => payment.ExternalPaymentId)
                .IsUnique()
                .HasFilter("[ExternalPaymentId] IS NOT NULL");

            builder.HasIndex(payment => payment.ExternalPreferenceId)
                .IsUnique()
                .HasFilter("[ExternalPreferenceId] IS NOT NULL");

            builder.HasIndex(payment => payment.ExternalReference)
                .IsUnique()
                .HasFilter("[ExternalReference] IS NOT NULL");

            builder.HasIndex(payment => payment.IdempotencyKey)
                .IsUnique()
                .HasFilter("[IdempotencyKey] IS NOT NULL");

            builder.HasIndex(payment => payment.ConceptKey)
                .IsUnique();
        }
    }
}