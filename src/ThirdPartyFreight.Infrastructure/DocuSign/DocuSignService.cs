using ThirdPartyFreight.Application.Abstractions.Caching;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Client.Auth;
using DocuSign.eSign.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThirdPartyFreight.Application.Abstractions.DocuSign;
using static DocuSign.eSign.Client.Auth.OAuth.UserInfo;
using UserInfo = DocuSign.eSign.Client.Auth.OAuth.UserInfo;

namespace ThirdPartyFreight.Infrastructure.DocuSign;

internal sealed class DocuSignService(
    ILogger<DocuSignService> logger,
    ICacheService cache,
    IOptions<DocuSignOptions> configuration)
    : IDocuSignService
{
    
    public async Task<EnvelopeSummary> SendEnvelopeFromTemplate(
        string signerEmail, 
        string signerName, 
        string templateId, 
        string customerNumber,
        string companyName, 
        string shipSites)
    {
        var response = new EnvelopeSummary();
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        EnvelopeDefinition envelope = MakeEnvelope(signerEmail, signerName, templateId, customerNumber, companyName, shipSites);

        EnvelopeSummary? envelopeSummary = await envelopesApi.CreateEnvelopeAsync(await GetAccountId(), envelope);
        try
        {
            response = envelopeSummary;
        }
        catch (Exception ex)
        {
            logger.LogError("There was error sending DocuSign Envelope: {Error}",ex.Message);
        }

        return response;
    }

    public async Task<EnvelopesInformation> GetEnvelopesInformation(StatusRequest envelopeStatusRequest)
    {
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        var response = new EnvelopesInformation();
        var listStatusChangesOptions = new EnvelopesApi.ListStatusChangesOptions
        {
            userFilter = envelopeStatusRequest.UserFilter,
            envelopeIds = envelopeStatusRequest.EnvelopeIds,
            include = envelopeStatusRequest.IncludeInformation,
            orderBy = envelopeStatusRequest.OrderBy,
            fromToStatus = envelopeStatusRequest.FromStatus,
            status = envelopeStatusRequest.StatusToInclude,
            fromDate = envelopeStatusRequest.FromDate,
            toDate = envelopeStatusRequest.ToDate
        };
        try
        {
            EnvelopesInformation? envelopesInformation =
                await envelopesApi.ListStatusChangesAsync(await GetAccountId(), listStatusChangesOptions);
            response = envelopesInformation;
        }
        catch (Exception ex)
        {
            logger.LogError("There was a problem getting Envelope Information {Error}", ex.Message);
        }

        return response;
    }

    public async Task<RecipientsUpdateSummary> UpdateEnvelopeEmailSettings(
        string envelopeId, 
        string newEmailAddress, 
        string newName, 
        bool resendEnvelope = true)
    {
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        Recipients recipient = new()
        {
            Signers =
            [
                new Signer
                {
                    Email = newEmailAddress,
                    Name = newName,
                }
            ]
        };

        EnvelopesApi.UpdateRecipientsOptions options = new()
        {
            resendEnvelope = resendEnvelope.ToString(),
        };

        RecipientsUpdateSummary? recipientsUpdateSummary =
            await envelopesApi.UpdateRecipientsAsync(await GetAccountId(), envelopeId, recipient, options);

        return recipientsUpdateSummary;
    }

    public async Task<EnvelopeUpdateSummary> VoidEnvelope(string envelopeId, string reason, string status = "voided")
    {
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        var env = new Envelope
        {
            Status = status,
            VoidedReason = reason
        };

        EnvelopeUpdateSummary? response = await envelopesApi.UpdateAsync(await GetAccountId(), envelopeId, env);
        return response;
    }

    public async Task<EnvelopeFormData> GetEnvelopeFormData(string envelopeId)
    {
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        EnvelopeFormData? response = await envelopesApi.GetFormDataAsync(await GetAccountId(), envelopeId);
        return response;
    }

    public async Task<EnvelopeDocumentsResult> GetDocumentList(string envelopeId)
    {
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        EnvelopeDocumentsResult? response = await envelopesApi.ListDocumentsAsync(await GetAccountId(), envelopeId);
        return response;
    }

    public async Task<Stream> GetDocumentById(string envelopeId, string documentId)
    {
        EnvelopesApi envelopesApi = await InitializeEnvelopesApi();
        Stream? response = await envelopesApi.GetDocumentAsync(await GetAccountId(), envelopeId, documentId);
        return response;
    }

    private async Task<EnvelopesApi> InitializeEnvelopesApi()
    {
        AuthInfo accountInfo = await GetAccessInfo();
        DocuSignClient client = new(accountInfo.BaseUrl + "/restapi");
        client.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accountInfo.AuthToken);
        return new EnvelopesApi(client);
    }

    private async Task<AuthInfo> GetAccessInfo()
    {
        string? accessToken = await cache.GetAsync<string>("DocuSignAccessToken");
        AuthInfo? accountInfo = await cache.GetAsync<AuthInfo>("DocuSignAccountInfo");

        if (accessToken == null || accountInfo == null)
        {
            string? newAccessToken = JwtAuth.RetrieveAccessToken(configuration.Value.IntegrationId, configuration.Value.ImpersonatedUserId,
                configuration.Value.AuthUrl, configuration.Value.PrivateKeyFilePath);

            if (newAccessToken == null)
            {
                logger.LogError("Error retrieving docusign access token");
                throw new NullReferenceException("Error retrieving docusign access token");
            }

            Account? account = await GetAccountInformation(newAccessToken);

            await cache.SetAsync("DocuSignAccessToken", newAccessToken, TimeSpan.FromMinutes(60));

            if (account == null)
            {
                logger.LogError("Error retrieving docusign account information");
                throw new NullReferenceException("Error retrieving docusign account information");
            }

            var response = new AuthInfo
            {
                AccountId = account.AccountId,
                AuthToken = newAccessToken,
                BaseUrl = new Uri(account.BaseUri)
            };
            
            return response;
        }

        var authInfo = new AuthInfo{ AuthToken = accessToken, AccountId = accountInfo.AccountId, BaseUrl = accountInfo.BaseUrl};

        return authInfo;
    }
    
    private async Task<Account?> GetAccountInformation(string? accessToken)
    {
        DocuSignClient client = new();
        client.SetOAuthBasePath(configuration.Value.AuthUrl);
        UserInfo? userInfo = client.GetUserInfo(accessToken);
        if (userInfo != null)
        {
            Account? accountInfo = configuration.Value.IsDevelopment
                ? userInfo.Accounts.FirstOrDefault()
                : userInfo.Accounts.LastOrDefault();

            await cache.SetAsync("DocuSignAccountInfo", accountInfo, TimeSpan.FromMinutes(60));

            return accountInfo;
        }
        else
        {
            return null;
        }
    }

    private static EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string templateId,
        string customerNumber, string companyName, string shipSites)
    {
        Text textCustomerNumber = new()
        {
            TabLabel = "prefill-customerNumber",
            Value = customerNumber
        };
        Text textCompanyName = new()
        {
            TabLabel = "prefill-companyName",
            Value = companyName
        };

        Text textShipToSites = new()
        {
            TabLabel = "prefill-siteNumbers",
            Value = shipSites
        };

        Tabs signerTabs = new()
        {
            TextTabs = [textCompanyName, textShipToSites, textCustomerNumber]
        };

        TemplateRole signer1 = new()
        {
            Email = signerEmail,
            Name = signerName,
            RoleName = "client",
            Tabs = signerTabs
        };

        EnvelopeDefinition env = new()
        {
            TemplateId = templateId,
            TemplateRoles = [signer1],
            Status = "sent"
        };

        return env;
    }

    private async Task<string> GetAccountId()
    {
        AuthInfo accountInfo = await GetAccessInfo();
        return accountInfo.AccountId;
    }
}
