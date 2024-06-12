using Microsoft.Graph.Models;

namespace ThirdPartyFreight.Application.Abstractions.MicrosoftGraph;
public interface IMicrosoftGraphService
{
    Task<User?> GetUser(string userPrincipalName);

    Task<(IEnumerable<Site>?, IEnumerable<Site>?)> GetSharepointSites();
}
