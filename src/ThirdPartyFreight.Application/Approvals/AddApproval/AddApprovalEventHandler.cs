using System.Net.Http.Headers;
using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Approvals.Events;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

internal sealed class AddApprovalEventHandler(
    IApprovalRepository approvalRepository,
    IWorkFlowTaskRepository workFlowTaskRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork,
    ILogger<AddApprovalEventHandler> logger) : INotificationHandler<ApprovalCreatedDomainEvent>
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
        using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:5001/elsa/api/workflow-definitions/b9abbad0024d0511/execute");
        request.Headers.TryAddWithoutValidation("Authorization", "ApiKey 00000000-0000-0000-0000-000000000000");
        string jsonContent = "{\"input\": {\"Approval\": {\"Id\": " + response.AgreementId + "}}}";
        request.Content = new StringContent(jsonContent);
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json"); 
        
        HttpResponseMessage httpResponse = await httpClient.SendAsync(request, cancellationToken);
        
        // response should not be null
        
        if (httpResponse.IsSuccessStatusCode)
        {
            
            // Update Approval Record
            IEnumerable<WorkFlowTask> workFlow =
                await workFlowTaskRepository.GetWorkFlowTaskAsyncByAgreementId(response.AgreementId, cancellationToken);
            Guid workflowId = workFlow.FirstOrDefault(x => x.AgreementId == response.Id)!.Id;
            logger.LogInformation("Updating Approval Record {ApprovalId}", notification.ApprovalId);
            
            Approval.Update(
                response,
                workflowId,
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
