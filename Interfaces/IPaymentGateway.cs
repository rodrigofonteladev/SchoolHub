using SchoolHub.Common;
using SchoolHub.Common.Enums;

namespace SchoolHub.Interfaces
{
    public interface IPaymentGateway
    {
        Task<Result<PaymentGatewayCheckout>> CreateCheckoutAsync(
            PaymentGatewayCheckoutRequest request,
            CancellationToken cancellationToken = default);

        Task<Result<PaymentGatewayPayment>> GetPaymentAsync(
            string externalPaymentId,
            CancellationToken cancellationToken = default);
    }

    public sealed record PaymentGatewayCheckoutRequest(
        int PaymentId,
        decimal Amount,
        string Currency,
        string Description,
        string ExternalReference,
        string IdempotencyKey);

    public sealed record PaymentGatewayCheckout(
        string Provider,
        string ExternalPreferenceId,
        string CheckoutUrl);

    public sealed record PaymentGatewayPayment(
        string Provider,
        string ExternalPaymentId,
        string? ExternalReference,
        PaymentStatus Status,
        decimal Amount,
        string Currency);
}