using Newtonsoft.Json;

namespace MidCapERP.Infrastructure.Identity.Models
{
    [JsonObject("token")]
    public class TokenConfiguration
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("expiry")]
        public int Expiry { get; set; }

        [JsonProperty("refreshExpiry")]
        public int RefreshExpiry { get; set; }
    }
}