namespace ThirdPartyFreight.Infrastructure.DocuSign;

public sealed class AuthInfo(string authToken, string accountId, Uri baseUrl)
{
    public string AuthToken { get; init; } = authToken;
    public string AccountId { get; init; } = accountId;
    public Uri BaseUrl { get; init; } = baseUrl;
}
