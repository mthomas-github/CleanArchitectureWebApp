using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Newtonsoft.Json;
using ThirdPartyFreight.Application.Abstractions.Elsa;
using ThirdPartyFreight.Application.Approvals.AddApproval;
using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Infrastructure.Elsa;
public class ElsaService : IElsaService
{
    public ElsaService(ILogger<ElsaService> logger, IOptions<ElsaServerOptions> options, HttpClient httpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        // Log the BaseAddress to check if it's set
        if (_httpClient.BaseAddress == null)
        {
            _logger.LogWarning("HttpClient BaseAddress is not set.");
        }
        else
        {
            _logger.LogInformation("HttpClient BaseAddress: {BaseAddress}", _httpClient.BaseAddress);
        }
    }

    private readonly ElsaServerOptions _options;
    private readonly ILogger<ElsaService> _logger;
    private readonly HttpClient _httpClient;

    public async Task ExecuteTask(string agreementId, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Execute Task For Agreement {Agreement}", agreementId);
            var url = new Uri($"workflow-definitions/{_options.WfDefinitionId}/execute", UriKind.Relative);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            string jsonContent = "{\"input\": {\n\"Approval\": {\n\"AgreementId\": \"" + agreementId +
                                 "\"\n}\n}\n}";
            request.Content = new StringContent(jsonContent, null, "application/json");
            HttpResponseMessage httpResponse = await _httpClient.SendAsync(request, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            _logger.LogInformation("Task Executed Completed For Agreement {Agreement}", agreementId);
        }
        catch(HttpRequestException  exception)
        {
            _logger.LogError("There was issue HttpRequestException {ErrorMessage} - {Error}", exception.Message, exception);
            throw new ServiceException("Error With Elsa Server");
        }
        catch (Exception exception)
        {
            _logger.LogError("There was issue Execute Elsa Server see {Error}", exception.Message);
            throw new ServiceException("Error With Elsa Server");
        }
    }

    public async Task CompleteTask(string taskId, object? result = default, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Complete Task {TaskId}", taskId);
            var url = new Uri($"tasks/{taskId}/complete", UriKind.Relative);
            var request = new { Result = result };
            await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            _logger.LogInformation("Task {TaskId} Completed", taskId);
        }
        catch(HttpRequestException  exception)
        {
            _logger.LogError("There was issue HttpRequestException {ErrorMessage} - {Error}", exception.Message, exception);
            throw new ServiceException("Error With Elsa Server");
        }
        catch (Exception exception)
        {
            _logger.LogError("There was issue Execute Elsa Server see {Error}", exception.Message);
            throw new ServiceException("Error With Elsa Server");
        }
    }

    public async Task DeleteWfInstance(string processId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Removing Instance {ProcessId}", processId);
            var url = new Uri($"workflow-instances/{processId}", UriKind.Relative);
            await _httpClient.DeleteAsync(url, cancellationToken);
            _logger.LogInformation("Removing Instance {ProcessId} was completed", processId);
        }
        catch(HttpRequestException  exception)
        {
            _logger.LogError("There was issue HttpRequestException {ErrorMessage} - {Error}", exception.Message, exception);
            throw new ServiceException("Error With Elsa Server");
        }
        //catch (TaskCanceledException exception
        catch (Exception exception)
        {
            _logger.LogError("There was issue Execute Elsa Server see {Error}", exception.Message);
            throw new ServiceException("Error With Elsa Server");
        }
    }
}
