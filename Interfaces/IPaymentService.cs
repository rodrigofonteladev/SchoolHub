using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Payment;

namespace SchoolHub.Interfaces
{
    public interface IPaymentService
    {
        Task<List<ListPaymentViewModel>> GetAllAsync();
        Task<Result> CreateEnrollmentObligationAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default);
        Task<Result> CreatePensionObligationsAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default);
        Task<Result<PaymentCheckoutViewModel>> CreateCheckoutAsync(
            int paymentId,
            CancellationToken cancellationToken = default);
        Task<Result> ConfirmPaymentAsync(
            string externalPaymentId,
            CancellationToken cancellationToken = default,
            string? expectedExternalReference = null);
    }
}
