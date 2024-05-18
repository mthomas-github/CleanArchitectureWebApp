using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Elsa;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Approvals.Events;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

internal sealed class AddApprovalDomainEventHandler(
    IApprovalRepository approvalRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork,
    IElsaService elsaService,
    ILogger<AddApprovalDomainEventHandler> logger) : INotificationHandler<ApprovalCreatedDomainEvent>
{
    public async Task Handle(ApprovalCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Pull Approval Record
        logger.LogInformation("Pulling Approval Record {ApprovalId}", notification.ApprovalId);

        Approval? response = await approvalRepository.GetByIdAsync(notification.ApprovalId, cancellationToken);

        if (response is null)
        {
            // Log Error
            logger.LogError("Approval Record {ApprovalId} not found", notification.ApprovalId);
            return;
        }

        // Call Else WorkFlow
        logger.LogInformation("Calling Else Workflow for Approval Record {ApprovalId}", notification.ApprovalId);
        // Call Else Workflow

        ElsaWorkFlowResponse elsaResponse =
            await elsaService.ExecuteTask(response.AgreementId.ToString(), cancellationToken);

        logger.LogInformation("Updating Approval Record {ApprovalId}", notification.ApprovalId);

        Approval.Update(
            response,
            elsaResponse.workflowState.bookmarks[0].payload.taskId,
            dateTimeProvider.UtcNow,
            null,
            null,
            null,
            null,
            null,
            null);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Approval Record {ApprovalId} saved", notification.ApprovalId);
    }
}
