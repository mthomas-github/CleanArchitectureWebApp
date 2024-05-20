using Newtonsoft.Json;

namespace ThirdPartyFreight.Application.Abstractions.PowerAutomate;

public sealed class AuthResponse
{
    [JsonProperty("token_type")]
    public string TokenType { get; init; }
    [JsonProperty("expire_in")]
    public string ExpiresIn { get; init; }
    [JsonProperty("ext_expires_in")]
    public string ExtExpiresIn { get; init; }
    [JsonProperty("access_token")]
    public string AccessToken { get; init; }
}
