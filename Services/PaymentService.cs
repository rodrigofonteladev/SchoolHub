using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolHub.Common;
using SchoolHub.Common.Enums;
using SchoolHub.Common.Settings;
using SchoolHub.Data;
using SchoolHub.Interfaces;
using SchoolHub.Models.Entities;
using SchoolHub.Models.ViewModels.Payment;

namespace SchoolHub.Services
{
    public class PaymentService(
        AppDbContext context,
        IPaymentGateway paymentGateway,
        IOptions<PaymentSettings> paymentOptions,
        ILogger<PaymentService> logger) : IPaymentService
    {
        private readonly PaymentSettings settings = paymentOptions.Value;

        public async Task<List<ListPaymentViewModel>> GetAllAsync()
        {
            return await context.Payments
                .AsNoTracking()
                .OrderByDescending(payment => payment.CreatedAt)
                .Select(payment => new ListPaymentViewModel
                {
                    Id = payment.Id,
                    EnrollmentId = payment.EnrollmentId,
                    StudentName = $"{payment.Enrollment.Student.FullName} {payment.Enrollment.Student.PaternalSurname} {payment.Enrollment.Student.MaternalSurname}",
                    PaymentType = payment.PaymentType,
                    PaymentStatus = payment.PaymentStatus,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    AcademicYear = payment.AcademicYear,
                    InstallmentNumber = payment.InstallmentNumber,
                    CreatedAt = payment.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<Result> CreateEnrollmentObligationAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default)
        {
            var enrollment = await context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == enrollmentId, cancellationToken);

            if (enrollment is null)
            {
                return Result.Failure("The enrollment does not exist.");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return Result.Failure("A cancelled enrollment cannot generate a payment obligation.");
            }

            var conceptKey = $"ENROLLMENT:{enrollment.Id}";
            if (await context.Payments.AnyAsync(payment => payment.ConceptKey == conceptKey, cancellationToken))
            {
                return Result.Success();
            }

            await context.Payments.AddAsync(new Payment
            {
                EnrollmentId = enrollment.Id,
                PaymentType = PaymentType.Enrollment,
                PaymentStatus = PaymentStatus.Pending,
                Amount = settings.EnrollmentAmount,
                Currency = settings.Currency,
                ConceptKey = conceptKey
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> CreatePensionObligationsAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default)
        {
            var enrollment = await context.Enrollments
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == enrollmentId, cancellationToken);

            if (enrollment is null)
            {
                return Result.Failure("The enrollment does not exist.");
            }

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                return Result.Failure("A cancelled enrollment cannot generate pension obligations.");
            }

            for (var installmentNumber = 1; installmentNumber <= settings.PensionInstallments; installmentNumber++)
            {
                var conceptKey = $"PENSION:{enrollment.Id}:{enrollment.EnrolledYear}:{installmentNumber}";
                var exists = await context.Payments.AnyAsync(
                    payment => payment.ConceptKey == conceptKey,
                    cancellationToken);

                if (!exists)
                {
                    await context.Payments.AddAsync(new Payment
                    {
                        EnrollmentId = enrollment.Id,
                        PaymentType = PaymentType.Pension,
                        PaymentStatus = PaymentStatus.Pending,
                        Amount = settings.PensionAmount,
                        Currency = settings.Currency,
                        AcademicYear = enrollment.EnrolledYear,
                        InstallmentNumber = installmentNumber,
                        ConceptKey = conceptKey
                    }, cancellationToken);
                }
            }

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> ConfirmPaymentAsync(
            string externalPaymentId,
            CancellationToken cancellationToken = default,
            string? expectedExternalReference = null)
        {
            if (string.IsNullOrWhiteSpace(externalPaymentId))
            {
                return Result.Failure("The external payment id is required.");
            }

            var gatewayResult = await paymentGateway.GetPaymentAsync(externalPaymentId, cancellationToken);
            if (!gatewayResult.Succeeded || gatewayResult.Value is null)
            {
                logger.LogWarning(
                    "Payment confirmation failed while querying Mercado Pago payment {ExternalPaymentId}: {Error}",
                    externalPaymentId,
                    gatewayResult.Error);
                return Result.Failure(gatewayResult.Error ?? "The payment could not be verified.");
            }

            var gatewayPayment = gatewayResult.Value;
            var referenceForLookup = gatewayPayment.ExternalReference ?? expectedExternalReference;
            var payment = await context.Payments
                .FirstOrDefaultAsync(item =>
                    item.ExternalPaymentId == gatewayPayment.ExternalPaymentId ||
                    item.ExternalReference == referenceForLookup,
                    cancellationToken);

            if (payment is null)
            {
                logger.LogWarning(
                    "Payment {ExternalPaymentId} was verified by Mercado Pago but no local payment matched. Gateway reference: {GatewayReference}; Expected reference: {ExpectedReference}.",
                    externalPaymentId,
                    gatewayPayment.ExternalReference,
                    expectedExternalReference);
                return Result.Failure("The local payment associated with the external payment was not found.");
            }

            if (!string.IsNullOrWhiteSpace(payment.ExternalReference) &&
                !string.IsNullOrWhiteSpace(gatewayPayment.ExternalReference) &&
                !string.Equals(payment.ExternalReference, gatewayPayment.ExternalReference, StringComparison.Ordinal))
            {
                logger.LogWarning(
                    "Payment {PaymentId} reference mismatch. Local: {LocalReference}; Gateway: {GatewayReference}.",
                    payment.Id,
                    payment.ExternalReference,
                    gatewayPayment.ExternalReference);
                return Result.Failure("The external payment reference does not match the local payment.");
            }

            if (payment.Amount != gatewayPayment.Amount ||
                !string.Equals(payment.Currency, gatewayPayment.Currency, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning(
                    "Payment {PaymentId} amount or currency mismatch. Local: {LocalAmount} {LocalCurrency}; Gateway: {GatewayAmount} {GatewayCurrency}.",
                    payment.Id,
                    payment.Amount,
                    payment.Currency,
                    gatewayPayment.Amount,
                    gatewayPayment.Currency);
                return Result.Failure("The verified payment amount or currency does not match the local payment.");
            }

            payment.Provider = gatewayPayment.Provider;
            payment.ExternalPaymentId = gatewayPayment.ExternalPaymentId;
            payment.PaymentStatus = gatewayPayment.Status;

            if (gatewayPayment.Status == PaymentStatus.Paid && payment.PaidAt is null)
            {
                payment.PaidAt = DateTime.UtcNow;
            }

            if (gatewayPayment.Status == PaymentStatus.Cancelled && payment.CancelledAt is null)
            {
                payment.CancelledAt = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Payment {PaymentId} updated from Mercado Pago payment {ExternalPaymentId} with status {Status}.",
                payment.Id,
                gatewayPayment.ExternalPaymentId,
                gatewayPayment.Status);

            if (gatewayPayment.Status == PaymentStatus.Paid && payment.PaymentType == PaymentType.Enrollment)
            {
                return await CreatePensionObligationsAsync(payment.EnrollmentId, cancellationToken);
            }

            return Result.Success();
        }

        public async Task<Result<PaymentCheckoutViewModel>> CreateCheckoutAsync(
            int paymentId,
            CancellationToken cancellationToken)
        {
            var payment = await context.Payments
                .Include(item => item.Enrollment)
                .ThenInclude(enrollment => enrollment.Student)
                .FirstOrDefaultAsync(item => item.Id == paymentId, cancellationToken);

            if (payment is null)
            {
                return Result<PaymentCheckoutViewModel>.Failure("The payment obligation does not exist.");
            }

            if (payment.PaymentStatus == PaymentStatus.Paid)
            {
                return Result<PaymentCheckoutViewModel>.Failure("The payment obligation has already been paid.");
            }

            if (payment.PaymentStatus is PaymentStatus.Cancelled or PaymentStatus.Refunded)
            {
                return Result<PaymentCheckoutViewModel>.Failure("This payment obligation cannot be paid.");
            }

            if (!string.IsNullOrWhiteSpace(payment.CheckoutUrl))
            {
                return Result<PaymentCheckoutViewModel>.Success(new PaymentCheckoutViewModel
                {
                    PaymentId = payment.Id,
                    CheckoutUrl = payment.CheckoutUrl
                });
            }

            payment.ExternalReference ??= $"PAYMENT-{payment.Id}";
            payment.IdempotencyKey ??= $"payment-{payment.Id}-{Guid.NewGuid():N}";
            await context.SaveChangesAsync(cancellationToken);

            var gatewayResult = await paymentGateway.CreateCheckoutAsync(
                new PaymentGatewayCheckoutRequest(
                    payment.Id,
                    payment.Amount,
                    payment.Currency,
                    BuildDescription(payment.PaymentType, payment.Enrollment),
                    payment.ExternalReference,
                    payment.IdempotencyKey),
                cancellationToken);

            if (!gatewayResult.Succeeded || gatewayResult.Value is null)
            {
                return Result<PaymentCheckoutViewModel>.Failure(gatewayResult.Error ?? "The checkout could not be created.");
            }

            payment.Provider = gatewayResult.Value.Provider;
            payment.ExternalPreferenceId = gatewayResult.Value.ExternalPreferenceId;
            payment.CheckoutUrl = gatewayResult.Value.CheckoutUrl;
            await context.SaveChangesAsync(cancellationToken);

            return Result<PaymentCheckoutViewModel>.Success(new PaymentCheckoutViewModel
            {
                PaymentId = payment.Id,
                CheckoutUrl = payment.CheckoutUrl
            });
        }

        private static string BuildDescription(PaymentType type, Models.Entities.Enrollment enrollment)
        {
            var studentName = $"{enrollment.Student.FullName} {enrollment.Student.PaternalSurname} {enrollment.Student.MaternalSurname}";
            return type == PaymentType.Enrollment
                ? $"Matrícula - {studentName}"
                : $"Pensión - {studentName}";
        }
    }
}
