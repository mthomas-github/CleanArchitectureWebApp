using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThirdPartyFreight.Application.Abstractions.PowerAutomate;

namespace ThirdPartyFreight.Infrastructure.PowerAutomate;

public class PowerAutomateService(IOptions<PowerAutomateOptions> options, ILogger<PowerAutomateService> logger)
    : IPowerAutomateService
{
    private readonly PowerAutomateOptions _options = options.Value;

    private async Task<AuthResponse> GetAuthToken()
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, _options.AuthTokenUrl);
        var content = new MultipartFormDataContent
        {
#pragma warning disable CA2000
            { new StringContent("client_credentials"), "grant_type" },

            { new StringContent(_options.ClientId), "client_id" },
            { new StringContent(_options.ClientSecret), "client_secret" },
            { new StringContent(_options.Scope), "scope" }
#pragma warning restore CA2000
        };
        request.Content = content;
        HttpResponseMessage response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }
        else
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to get auth token: {response.StatusCode}, {errorContent}");
        }
    }

    public async Task TriggerFlow(Uri flowUrl, FlowRequest flowRequest)
    {
        try
        {
            string data = flowRequest.JsonDataString;
            AuthResponse authResponse = await GetAuthToken();
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, flowUrl);
            request.Headers.Add("Authorization", $"Bearer {authResponse.AccessToken}");
            var content =
                new StringContent(data, null, "application/json");
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException httpRequestException)
        {
            logger.LogCritical("Tried Starting PowerAutomate But it Failed {Error}",
                httpRequestException.InnerException);
        }
        catch (Exception exception)
        {
            logger.LogCritical("There was a general exception {Error}", exception.Message);
        }
    }
}
