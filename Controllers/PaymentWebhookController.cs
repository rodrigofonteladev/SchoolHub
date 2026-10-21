using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Payment;
using SchoolHub.Services;

namespace SchoolHub.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [Route("api/payments/webhook/mercadopago")]
    public class PaymentWebhookController(
        IPaymentService paymentService,
        MercadoPagoWebhookSignatureValidator signatureValidator,
        ILogger<PaymentWebhookController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Receive(
            [FromBody] MercadoPagoWebhookRequest request,
            CancellationToken cancellationToken)
        {
            var topic = request.Type ?? Request.Query["topic"].FirstOrDefault();

            // Checkout Pro can also emit merchant_order notifications through the
            // legacy IPN mechanism. They are not payment notifications and do not
            // use the Webhook signature validation flow.
            if (string.Equals(topic, "merchant_order", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(topic, "topic_merchant_order_wh", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("Ignoring Mercado Pago merchant order notification.");
                return Ok(new { received = true });
            }

            var paymentId = request.Data?.Id ?? Request.Query["data.id"].FirstOrDefault() ?? Request.Query["id"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(paymentId))
            {
                return BadRequest(new { error = "The payment id is required." });
            }

            if (!signatureValidator.IsValid(Request, paymentId))
            {
                logger.LogWarning("Invalid Mercado Pago webhook signature for payment {PaymentId}.", paymentId);
                return Unauthorized();
            }

            var result = await paymentService.ConfirmPaymentAsync(paymentId, cancellationToken);
            if (!result.Succeeded)
            {
                logger.LogWarning("Payment confirmation failed for {PaymentId}: {Error}", paymentId, result.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = result.Error });
            }

            return Ok(new { received = true });
        }
    }
}
