using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;

namespace ThirdPartyFreight.Infrastructure.DocuSign;

public static class JwtAuth
{
#pragma warning disable CA1054
    public static string? RetrieveAccessToken(string clientId, string impersonatedUserId, string authUrl,
#pragma warning restore CA1054
        string privateKeyFilePath)
    {
        OAuth.OAuthToken? accessTokenObj = null;

        try
        {
            accessTokenObj = AuthenticateWithJwt("ESignature", clientId, impersonatedUserId, authUrl,
                ReadFileContent(privateKeyFilePath));
        }
        catch (ApiException apiExp)
        {
            if (apiExp.Message.Contains("consent_required"))
            {
                return apiExp.Message;
            }
        }

        return accessTokenObj!.access_token;
    }
    
    private static OAuth.OAuthToken AuthenticateWithJwt(string api, string clientId, string imperonatedUserId,
        string authServer, byte[] privateKeyBytes)
    {
        var docuSignClient = new DocuSignClient();
        ApiType apiType = Enum.Parse<ApiType>(api);
        var scopes = new List<string> { "signature", "impersonation" };

        return docuSignClient.RequestJWTUserToken(
            clientId,
            imperonatedUserId,
            authServer,
            privateKeyBytes,
            1,
            scopes);
    }
    
    private static byte[] ReadFileContent(string path)
    {
        return File.ReadAllBytes(path);
    }
}
