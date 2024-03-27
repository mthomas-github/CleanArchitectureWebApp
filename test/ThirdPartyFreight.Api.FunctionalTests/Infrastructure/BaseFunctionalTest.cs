using System.Net.Http.Json;
using ThirdPartyFreight.Api.Controllers.Users;
using ThirdPartyFreight.Application.Users.LogInUser;
using ThirdPartyFreight.Api.FunctionalTests.Users;

namespace ThirdPartyFreight.Api.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected async Task<string> GetAccessToken()
    {
        HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync(
            "api/v1/users/login",
            new LogInUserRequest(
                UserData.RegisterTestUserRequest.Email,
                UserData.RegisterTestUserRequest.Password));

        AccessTokenResponse? accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

        return accessTokenResponse!.AccessToken;
    }
}
