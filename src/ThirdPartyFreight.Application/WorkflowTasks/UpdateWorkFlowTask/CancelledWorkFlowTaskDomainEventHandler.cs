using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.WorkflowTask;
using ThirdPartyFreight.Domain.WorkflowTask.Events;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

public class CancelledWorkFlowTaskDomainEventHandler(
    IWorkFlowTaskRepository workFlowTaskRepository, 
    IApprovalRepository approvalRepository, 
    IAgreementRepository agreementRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ILogger<CancelledWorkFlowTaskDomainEventHandler> logger) 
    : INotificationHandler<WorkFlowTaskCancelledDomainEvent>
{
    public async Task Handle(WorkFlowTaskCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Executing {Name} Domain Event", nameof(UpdateWorkFlowTaskDomainEventHandler));
        logger.LogInformation("Pulling WorkflowTask Information For {WorkFlowTaskId}", notification.WorkFlowTaskId);
        WorkFlowTask? workFlowTask =
            await workFlowTaskRepository.GetByIdAsync(notification.WorkFlowTaskId, cancellationToken);
        // Step 2 Update Approval && If Complete MDM Update Agreement As Well
        if (workFlowTask is null)
        {
            logger.LogError("Unable To Get WorkFlowTask For {WorkFlowId}", notification.WorkFlowTaskId);
            throw new NullReferenceException();
        }
        logger.LogInformation("Completed Pulling Workflow Task For Task Id {WorkFlowTaskId}", notification.WorkFlowTaskId);
        logger.LogInformation("Pulling Approval Information For Task Id {TaskId}", workFlowTask.ExternalId);
        Approval? approval = await approvalRepository.GetByAgreementIdAsync(workFlowTask.AgreementId, cancellationToken);
        if (approval is null)
        {
            logger.LogError("Unable To Get Approval For {TaskId}", workFlowTask.ExternalId);
            throw new NullReferenceException();
        }
        logger.LogInformation("Completed Pulling Approval For Task Id {TaskId}", workFlowTask.ExternalId);

        if (workFlowTask.Voided)
        {          
            Agreement? agreement = await agreementRepository.GetByIdAsync(workFlowTask.AgreementId, cancellationToken);
            
            agreement?.SetStatus(Status.ApprovalRejected, dateTimeProvider.UtcNow);

            approval.SetUpdatedValues(
                workFlowTask.ExternalId,
                approval.FirstApprovalOnUtc,
                approval.FirstApprovalEndUtc,
                approval.SecondApprovalOnUtc,
                approval.SecondApprovalEndUtc,
                approval.ThirdApprovalOnUtc,
                approval.ThirdApprovalEndUtc,
                approval.CompletedOn,
                true
            );
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        logger.LogInformation("Finished Executing {Name} Domain Event", nameof(UpdateWorkFlowTaskDomainEventHandler));

    }
}
