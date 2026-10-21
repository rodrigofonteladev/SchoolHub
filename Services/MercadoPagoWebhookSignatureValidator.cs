using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using SchoolHub.Common.Settings;

namespace SchoolHub.Services
{
    public class MercadoPagoWebhookSignatureValidator(IOptions<MercadoPagoSettings> options)
    {
        private readonly MercadoPagoSettings settings = options.Value;

        public bool IsValid(HttpRequest request, string paymentId)
        {
            if (string.IsNullOrWhiteSpace(settings.WebhookSecret))
            {
                return settings.UseSandbox;
            }

            if (!request.Headers.TryGetValue("x-signature", out var signatureHeader) ||
                !request.Headers.TryGetValue("x-request-id", out var requestIdHeader))
            {
                return false;
            }

            var signatureParts = signatureHeader.ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(part => part.Split('=', 2, StringSplitOptions.TrimEntries))
                .Where(part => part.Length == 2)
                .ToDictionary(part => part[0], part => part[1], StringComparer.OrdinalIgnoreCase);

            if (!signatureParts.TryGetValue("ts", out var timestamp) ||
                !signatureParts.TryGetValue("v1", out var receivedSignature))
            {
                return false;
            }

            var manifest = $"id:{paymentId};request-id:{requestIdHeader};ts:{timestamp};";
            var key = Encoding.UTF8.GetBytes(settings.WebhookSecret);
            var data = Encoding.UTF8.GetBytes(manifest);
            var hash = HMACSHA256.HashData(key, data);
            var expectedSignature = Convert.ToHexString(hash).ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expectedSignature),
                Encoding.UTF8.GetBytes(receivedSignature));
        }
    }
}