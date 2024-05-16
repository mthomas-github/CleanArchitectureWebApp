using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Approvals.Events;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

internal sealed class AddApprovalDomainEventHandler(
    IApprovalRepository approvalRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork,
    ILogger<AddApprovalDomainEventHandler> logger) : INotificationHandler<ApprovalCreatedDomainEvent>
{
    public async Task Handle(ApprovalCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Pull Approval Record
        logger.LogInformation("Pulling Approval Record {ApprovalId}", notification.ApprovalId);
        
        Approval? response = await approvalRepository.GetByIdAsync(notification.ApprovalId, cancellationToken);
        
        if(response is null)
        {
            // Log Error
            logger.LogError("Approval Record {ApprovalId} not found", notification.ApprovalId);
            return;
        }

        // Call Else WorkFlow
        logger.LogInformation("Calling Else Workflow for Approval Record {ApprovalId}", notification.ApprovalId);
        // Call Else Workflow
        using var httpClient = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get,  "https://localhost:5001/elsa/api/workflow-definitions/b9abbad0024d0511/execute");
        request.Headers.Add("Authorization", "ApiKey 00000000-0000-0000-0000-000000000000");
        string jsonContent = "{\"input\": {\n\"Approval\": {\n\"AgreementId\": \"" + response.AgreementId + "\"\n}\n}\n}";
        request.Content = new StringContent(jsonContent, null, "application/json");;
        HttpResponseMessage httpResponse = await httpClient.SendAsync(request, cancellationToken);
        
        // response should not be null
        
        if (httpResponse.IsSuccessStatusCode)
        {
            
            // Update Approval Record
            // Read HTTP Response
             string responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
             ElsaWorkFlowResponse? data = JsonConvert.DeserializeObject<ElsaWorkFlowResponse>(responseBody);
             
             if(data is null) {
                 logger.LogError("Failed to deserialize Elsa Workflow Response");
                 throw new NullReferenceException("Failed to deserialize Elsa Workflow Response");
             }
             
             logger.LogInformation("Updating Approval Record {ApprovalId}", notification.ApprovalId);
            
            Approval.Update(
                response,
                data.workflowState.bookmarks[0].payload.taskId,
                dateTimeProvider.UtcNow,
                null,
                null,
                null,
                null,
                null,
                null);
        }
        else
        {
            // Log Error
            logger.LogError("Failed to call Else Workflow for Approval Record {ApprovalId}", notification.ApprovalId);
        }
        // Save Approval Record
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Approval Record {ApprovalId} saved", notification.ApprovalId);
    }
}
