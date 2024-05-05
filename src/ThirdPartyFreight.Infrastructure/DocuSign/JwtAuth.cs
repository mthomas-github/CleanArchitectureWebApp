using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;

namespace ThirdPartyFreight.Api;

public static class JwtAuth
{
    public static OAuth.OAuthToken AuthenticateWithJwt(string api, string clientId, string imperonatedUserId,
        string authServer, byte[] privateKeyBytes)
    {
        var docuSignClient = new DocuSignClient();
        var scopes = new List<string> { "signature", "impersonation" };

        return docuSignClient.RequestJWTUserToken(
            clientId,
            imperonatedUserId,
            authServer,
            privateKeyBytes,
            1,
            scopes);
    }
}
