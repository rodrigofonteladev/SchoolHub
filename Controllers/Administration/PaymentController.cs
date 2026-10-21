using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Payment;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class PaymentController(
        IPaymentService paymentService,
        ILogger<PaymentController> logger) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await paymentService.GetAllAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(int id, CancellationToken cancellationToken)
        {
            var result = await paymentService.CreateCheckoutAsync(id, cancellationToken);
            if (!result.Succeeded)
            {
                TempData["PaymentError"] = result.Error ?? "The checkout could not be created.";
                return RedirectToAction(nameof(Index));
            }

            return Redirect(result.Value!.CheckoutUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Success(
            string? external_reference,
            string? payment_id,
            string? collection_id,
            CancellationToken cancellationToken)
        {
            // The redirect is not a replacement for the webhook. It is a fallback
            // for cases where Mercado Pago completes the browser redirect before
            // delivering the asynchronous notification.
            var externalPaymentId = payment_id ?? collection_id;
            if (!string.IsNullOrWhiteSpace(externalPaymentId))
            {
                var confirmation = await paymentService.ConfirmPaymentAsync(
                    externalPaymentId,
                    cancellationToken,
                    external_reference);

                if (!confirmation.Succeeded)
                {
                    logger.LogWarning(
                        "Payment return confirmation failed for external payment {ExternalPaymentId}: {Error}",
                        externalPaymentId,
                        confirmation.Error);
                }
            }

            return View("Result", new PaymentResultViewModel
            {
                Title = "Payment submitted",
                Message = "Your payment was submitted. We are verifying its status with Mercado Pago.",
                ExternalReference = external_reference
            });
        }

        [HttpGet]
        public IActionResult Pending(string? external_reference) =>
            View("Result", new PaymentResultViewModel
            {
                Title = "Payment pending",
                Message = "Your payment is still being processed. Its status will be updated after verification.",
                ExternalReference = external_reference
            });

        [HttpGet]
        public IActionResult Failure(string? external_reference) =>
            View("Result", new PaymentResultViewModel
            {
                Title = "Payment not completed",
                Message = "The payment was not completed. You can return to payments and try again.",
                ExternalReference = external_reference
            });

    }
}
