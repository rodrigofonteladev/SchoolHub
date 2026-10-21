using System.Text.Json.Serialization;

namespace SchoolHub.Models.ViewModels.Payment
{
    public class MercadoPagoWebhookRequest
    {
        public string? Type { get; set; }
        public string? Action { get; set; }
        public MercadoPagoWebhookData? Data { get; set; }
    }

    public class MercadoPagoWebhookData
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}