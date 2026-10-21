namespace SchoolHub.Common.Settings
{
    public class MercadoPagoSettings
    {
        public const string SectionName = "MercadoPagoSettings";

        public string AccessToken { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.mercadopago.com/";
        public string PublicBaseUrl { get; set; } = "http://localhost:5095";
        public bool UseSandbox { get; set; } = true;
        public string WebhookSecret { get; set; } = string.Empty;
    }
}