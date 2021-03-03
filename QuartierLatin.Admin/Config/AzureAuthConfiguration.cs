namespace QuartierLatin.Admin.Config
{
    public class AzureAuthConfiguration
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrl { get; set; }
        public string LogoutRedirectUrl { get; set; }

        public void Deconstruct(out string tenantId, out string clientId, out string clientSecret,
            out string redirectUrl)
        {
            tenantId = TenantId;
            clientId = ClientId;
            clientSecret = ClientSecret;
            redirectUrl = RedirectUrl;
        }
    }
}