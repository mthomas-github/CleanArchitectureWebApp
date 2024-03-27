using ThirdPartyFreight.Application.Abstractions.Authentication;
using ThirdPartyFreight.Domain.Users;
using System.Net.Http.Json;
using ThirdPartyFreight.Infrastructure.Authentication.Models;

namespace ThirdPartyFreight.Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private const string PasswordCredentialType = "password";

    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var userRepresentationModel = UserRepresentationModel.FromUser(user);

            userRepresentationModel.Credentials =
            [
                new CredentialRepresentationModel
                {
                    Value = password,
                    Temporary = false,
                    Type = PasswordCredentialType
                }
            ];

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "users",
                userRepresentationModel,
                cancellationToken);

            return ExtractIdentityIdFromLocationHeader(response);
        }
        catch (HttpRequestException)
        {
            throw new InvalidOperationException("Unable to register user");
        }
    }

    private static string ExtractIdentityIdFromLocationHeader(
        HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;

        if (locationHeader is not null)
        {
            int userSegmentValueIndex = locationHeader.IndexOf(
                usersSegmentName,
                StringComparison.InvariantCultureIgnoreCase);

            string userIdentityId = locationHeader[(userSegmentValueIndex + usersSegmentName.Length)..];

            return userIdentityId;
        }

        throw new InvalidOperationException("Location header can't be null");
    }
}
