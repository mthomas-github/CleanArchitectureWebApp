using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.WorkflowTask;
using ThirdPartyFreight.Domain.WorkflowTask.Events;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

public class UpdateWorkFlowTaskDomainEventHandler(
    IWorkFlowTaskRepository workFlowTaskRepository,
    IApprovalRepository approvalRepository,
    IAgreementRepository agreementRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ILogger<UpdateWorkFlowTaskDomainEventHandler> logger) : INotificationHandler<WorkFlowTaskUpdatedDomainEvent>
{
    public async Task Handle(WorkFlowTaskUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Step 1 Get WorkFlow
        logger.LogInformation("Executing {Name} Domain Event", nameof(UpdateWorkFlowTaskDomainEventHandler));
        logger.LogInformation("Pulling WorkflowTask Information For {WorkFlowTaskId}", notification.WorkFlowId);
        WorkFlowTask? workFlowTask =
            await workFlowTaskRepository.GetByIdAsync(notification.WorkFlowId, cancellationToken);
        // Step 2 Update Approval && If Complete MDM Update Agreement As Well
        if (workFlowTask is null)
        {
            logger.LogError("Unable To Get WorkFlowTask For {WorkFlowId}", notification.WorkFlowId);
            throw new NullReferenceException();
        }
        logger.LogInformation("Completed Pulling Workflow Task For Task Id {WorkFlowTaskId}", notification.WorkFlowId);
        logger.LogInformation("Pulling Approval Information For Task Id {TaskId}", workFlowTask.ExternalId);
        Approval? approval = await approvalRepository.GetByAgreementIdAsync(workFlowTask.AgreementId, cancellationToken);
        
        if (approval is null)
        {
            logger.LogError("Unable To Get Approval For {TaskId}", workFlowTask.ExternalId);
            throw new NullReferenceException();
        }
        logger.LogInformation("Completed Pulling Approval For Task Id {TaskId}", workFlowTask.ExternalId);

        if (workFlowTask.Approver == ApproverType.MdmTeam)
        {
            Agreement? agreement = await agreementRepository.GetByIdAsync(workFlowTask.AgreementId, cancellationToken);
            if (agreement is null)
            {
                logger.LogError("Unable To Get Agreement For {AgreementId}", workFlowTask.AgreementId);
                throw new NullReferenceException();
            }

            logger.LogInformation("Updating Approval - {ApprovalId} & Agreements - {AgreementId} for Completion",
                approval.Id, agreement.Id);

            agreement.SetStatus(Status.Completed, dateTimeProvider.UtcNow);
            
            approval.SetUpdatedValues(workFlowTask.ExternalId, approval.FirstApprovalOnUtc,
                approval.FirstApprovalEndUtc, approval.SecondApprovalOnUtc, approval.SecondApprovalEndUtc,
                approval.ThirdApprovalOnUtc, workFlowTask.CompletedAt?.DateTime, workFlowTask.CompletedAt?.DateTime, false);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            
        }
        logger.LogInformation("Completed Executing {Name} Domain Event", nameof(UpdateWorkFlowTaskDomainEventHandler));
    }
}
