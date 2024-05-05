using Newtonsoft.Json;

namespace ThirdPartyFreight.Infrastructure.DocuSign;

public class AuthInfo
{
    [JsonProperty("account_id")]
    public string AccountId { get; set; }
    [JsonProperty("is_default")]
    public string IsDefault { get; set; }
    [JsonProperty("account_name")]
    public string AccountName { get; set; }
    public string AuthToken { get; set; }
    [JsonProperty("base_uri")]
    public Uri BaseUrl { get; set; }

}
