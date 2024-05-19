namespace ThirdPartyFreight.Application.Abstractions.PowerAutomate;

public sealed class AuthResponse
{
    public string TokenType { get; init; }
    public string ExpiresIn { get; init; }
    public string ExtExpiresIn { get; init; }
    public string AccessToken { get; init; }
}
