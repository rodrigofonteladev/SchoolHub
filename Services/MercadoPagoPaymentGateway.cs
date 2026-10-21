using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SchoolHub.Common;
using SchoolHub.Common.Enums;
using SchoolHub.Common.Settings;
using SchoolHub.Interfaces;

namespace SchoolHub.Services
{
    public class MercadoPagoPaymentGateway(
        HttpClient httpClient,
        IOptions<MercadoPagoSettings> options,
        ILogger<MercadoPagoPaymentGateway> logger) : IPaymentGateway
    {
        private readonly MercadoPagoSettings settings = options.Value;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<Result<PaymentGatewayCheckout>> CreateCheckoutAsync(
            PaymentGatewayCheckoutRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(settings.AccessToken))
            {
                return Result<PaymentGatewayCheckout>.Failure("Mercado Pago is not configured.");
            }

            var payload = new
            {
                items = new[]
                {
                    new
                    {
                        id = request.PaymentId.ToString(),
                        title = request.Description,
                        quantity = 1,
                        currency_id = request.Currency,
                        unit_price = request.Amount
                    }
                },
                external_reference = request.ExternalReference,
                back_urls = new
                {
                    success = BuildUrl("/Payment/Success"),
                    pending = BuildUrl("/Payment/Pending"),
                    failure = BuildUrl("/Payment/Failure")
                },
                auto_return = "approved"
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "checkout/preferences")
            {
                Content = JsonContent.Create(payload)
            };
            httpRequest.Headers.Add("X-Idempotency-Key", request.IdempotencyKey);

            try
            {
                using var response = await httpClient.SendAsync(httpRequest, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("Mercado Pago preference creation failed with status {StatusCode}.", response.StatusCode);
                    return Result<PaymentGatewayCheckout>.Failure("Mercado Pago could not create the checkout.");
                }

                var preference = JsonSerializer.Deserialize<PreferenceResponse>(responseBody, JsonOptions);
                var checkoutUrl = settings.UseSandbox
                    ? preference?.SandboxInitPoint ?? preference?.InitPoint
                    : preference?.InitPoint;

                if (preference?.Id is null || string.IsNullOrWhiteSpace(checkoutUrl))
                {
                    return Result<PaymentGatewayCheckout>.Failure("Mercado Pago returned an incomplete checkout response.");
                }

                return Result<PaymentGatewayCheckout>.Success(new PaymentGatewayCheckout(
                    "MercadoPago",
                    preference.Id,
                    checkoutUrl));
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Mercado Pago preference request failed.");
                return Result<PaymentGatewayCheckout>.Failure("Mercado Pago is currently unavailable.");
            }
        }

        public async Task<Result<PaymentGatewayPayment>> GetPaymentAsync(
            string externalPaymentId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(settings.AccessToken))
            {
                return Result<PaymentGatewayPayment>.Failure("Mercado Pago is not configured.");
            }

            try
            {
                var response = await httpClient.GetAsync(
                    $"v1/payments/{Uri.EscapeDataString(externalPaymentId)}",
                    cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning("Mercado Pago payment query failed with status {StatusCode}.", response.StatusCode);
                    return Result<PaymentGatewayPayment>.Failure("Mercado Pago could not verify the payment.");
                }

                var payment = JsonSerializer.Deserialize<PaymentResponse>(responseBody, JsonOptions);
                var paymentId = payment is null ? null : ReadJsonValueAsString(payment.Id);

                if (payment is null || string.IsNullOrWhiteSpace(paymentId))
                {
                    logger.LogWarning("Mercado Pago payment response did not contain a valid payment id.");
                    return Result<PaymentGatewayPayment>.Failure("Mercado Pago returned an incomplete payment response.");
                }

                if (payment.TransactionAmount is null || string.IsNullOrWhiteSpace(payment.CurrencyId))
                {
                    logger.LogWarning(
                        "Mercado Pago payment response was incomplete. HasId: {HasId}, HasAmount: {HasAmount}, HasCurrency: {HasCurrency}.",
                        !string.IsNullOrWhiteSpace(paymentId),
                        payment?.TransactionAmount is not null,
                        !string.IsNullOrWhiteSpace(payment?.CurrencyId));
                    return Result<PaymentGatewayPayment>.Failure("Mercado Pago returned an incomplete payment response.");
                }

                return Result<PaymentGatewayPayment>.Success(new PaymentGatewayPayment(
                    "MercadoPago",
                    paymentId,
                    payment.ExternalReference,
                    MapStatus(payment.Status),
                    payment.TransactionAmount.Value,
                    payment.CurrencyId));
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Mercado Pago payment query failed.");
                return Result<PaymentGatewayPayment>.Failure("Mercado Pago is currently unavailable.");
            }
        }

        private string BuildUrl(string path) =>
            $"{settings.PublicBaseUrl.TrimEnd('/')}{path}";

        private static PaymentStatus MapStatus(string? status) => status?.ToLowerInvariant() switch
        {
            "approved" => PaymentStatus.Paid,
            "pending" or "in_process" or "in_mediation" => PaymentStatus.Pending,
            "rejected" => PaymentStatus.Failed,
            "cancelled" => PaymentStatus.Cancelled,
            "refunded" or "charged_back" => PaymentStatus.Refunded,
            _ => PaymentStatus.Pending
        };

        private sealed class PreferenceResponse
        {
            public string? Id { get; set; }

            [JsonPropertyName("init_point")]
            public string? InitPoint { get; set; }

            [JsonPropertyName("sandbox_init_point")]
            public string? SandboxInitPoint { get; set; }
        }

        private sealed class PaymentResponse
        {
            [JsonPropertyName("id")]
            public JsonElement Id { get; set; }

            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("external_reference")]
            public string? ExternalReference { get; set; }

            [JsonPropertyName("transaction_amount")]
            public decimal? TransactionAmount { get; set; }

            [JsonPropertyName("currency_id")]
            public string? CurrencyId { get; set; }
        }

        private static string? ReadJsonValueAsString(JsonElement value)
        {
            return value.ValueKind switch
            {
                JsonValueKind.String => value.GetString(),
                JsonValueKind.Number => value.GetRawText(),
                _ => null
            };
        }
    }
}
