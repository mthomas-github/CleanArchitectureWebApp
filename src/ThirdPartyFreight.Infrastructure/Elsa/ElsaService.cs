using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Newtonsoft.Json;
using ThirdPartyFreight.Application.Abstractions.Elsa;
using ThirdPartyFreight.Application.Approvals.AddApproval;

namespace ThirdPartyFreight.Infrastructure.Elsa;

internal sealed class ElsaService(ILogger<ElsaService> logger, IOptions<ElsaServerOptions> configuration) : IElsaService
{
    private readonly ElsaServerOptions _options = configuration.Value;
    
    public async Task<ElsaWorkFlowResponse> ExecuteTask(string agreementId, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            string requestUrl = $"{_options.ApiBaseUrl}/workflow-definitions/{_options.WfDefinitionId}/execute";
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", $"ApiKey {_options.ApiKey}");
            string jsonContent = "{\"input\": {\n\"Approval\": {\n\"AgreementId\": \"" + agreementId +
                                 "\"\n}\n}\n}";
            request.Content = new StringContent(jsonContent, null, "application/json");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(request, cancellationToken);
            string responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            ElsaWorkFlowResponse? data = JsonConvert.DeserializeObject<ElsaWorkFlowResponse>(responseBody);
            return data;
        }
        catch (Exception exception)
        {
            logger.LogError("There was issue Execute Elsa Server see {Error}", exception.Message);
            throw new ServiceException("Error With Elsa Server");
        }
    }

    public Task CompleteTask(string taskId, object? result = default, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
