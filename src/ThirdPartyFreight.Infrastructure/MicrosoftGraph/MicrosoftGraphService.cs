using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using ThirdPartyFreight.Application.Abstractions.MicrosoftGraph;

namespace ThirdPartyFreight.Infrastructure.MicrosoftGraph;

public class MicrosoftGraphService(string tenantId, string clientId, string clientSecret) : IMicrosoftGraphService
{
    private GraphServiceClient GraphClient { get; set; } = CreateGraphClient(tenantId, clientId, clientSecret);
    private static GraphServiceClient CreateGraphClient(string tenantId, string clientId, string clientSecret)
    {
        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };
        var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

        string[] scopes = ["https://graph.microsoft.com/.default"];

        return new GraphServiceClient(clientSecretCredential, scopes);
    }

    public async Task<User?> GetUser(string userPrincipalName)
    {
        return await GraphClient.Users[userPrincipalName].GetAsync();
    }

    public async Task<(IEnumerable<Site>?, IEnumerable<Site>?)> GetSharepointSites()
    {
        List<Site>? sites = (await GraphClient.Sites.GetAllSites.GetAsGetAllSitesGetResponseAsync())?.Value;
        if (sites == null)
        {
            return (null, null);
        }

        sites.RemoveAll(x => string.IsNullOrEmpty(x.DisplayName));

        var spSites = new List<Site>();
        var oneDriveSites = new List<Site>();

        foreach (Site site in sites)
        {
            if (site == null)
            {
                continue;
            }

            string[]? compare = site.WebUrl?.Split(site.SiteCollection?.Hostname)[1].Split("/");
            if (compare != null && (compare.All(x => !string.IsNullOrEmpty(x)) || compare.Length < 1))
            {
                continue;
            }

            if (compare?[1] == "sites" || string.IsNullOrEmpty(compare?[1]))
            {
                spSites.Add(site);
            } else if (compare[1] == "personal")
            {
                oneDriveSites.Add(site);
            }
        }

        return (spSites, oneDriveSites);

    }
}
