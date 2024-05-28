using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.PowerAutomate;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.WorkflowTask;
using ThirdPartyFreight.Domain.WorkflowTask.Events;

namespace ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;

public class AddWorkFlowTaskDomainEventHandler(
    IWorkFlowTaskRepository workFlowTaskRepository, 
    IApprovalRepository approvalRepository,
    IAgreementRepository agreementRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ILogger<AddWorkFlowTaskDomainEventHandler> logger) : 
    INotificationHandler<WorkFlowTaskCreatedDomainEvent>
{
    public async Task Handle(WorkFlowTaskCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Step 1 Get WorkFlow Task
        WorkFlowTask? workFlowTask = await workFlowTaskRepository.GetByIdAsync(notification.WorkFlowId, cancellationToken);

        if (workFlowTask != null)
        {
            // Step 2 Get The Approval & Agreement To Update
            Approval? approval = await approvalRepository.GetByAgreementIdAsync(workFlowTask.AgreementId, cancellationToken);
            Agreement? agreement = await agreementRepository.GetByIdAsync(workFlowTask.AgreementId, cancellationToken);
            if (approval != null && agreement != null)
            {
                DateTime? startTime = workFlowTask.CreatedAt.DateTime;
                switch (workFlowTask.Approver)
                {
                    case ApproverType.TpfTeam:
                        approval.SetUpdatedValues(
                            workFlowTask.ExternalId,
                            startTime,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            false);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        break;
                    case ApproverType.TmsTeam:
                        approval.SetUpdatedValues(
                            workFlowTask.ExternalId,
                            approval.FirstApprovalOnUtc,
                            approval.FirstApprovalEndUtc ?? startTime,
                            startTime,
                            null,
                            null,
                            null,
                            null,
                            false);
                        agreement.SetStatus(Status.PendingReviewTms, dateTimeProvider.UtcNow);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        break;
                    case ApproverType.MdmTeam:
                        approval.SetUpdatedValues(
                            workFlowTask.ExternalId,
                            approval.FirstApprovalOnUtc,
                            approval.FirstApprovalEndUtc,
                            approval.SecondApprovalOnUtc,
                            approval.SecondApprovalEndUtc ?? startTime,
                            startTime,
                            null,
                            null,
                            false);
                        agreement.SetStatus(Status.PendingReviewMdm, dateTimeProvider.UtcNow);
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                        break;
                    default:
#pragma warning disable CA2208
                        throw new ArgumentOutOfRangeException(nameof(workFlowTask), workFlowTask.Approver, "Unexpected ApproverType value");                }
#pragma warning restore CA2208
            }
            else
            {
                logger.LogError("Approval With AgreementId: {AgreementId} was not found", workFlowTask.AgreementId);
                throw new NullReferenceException();
            }
        }
        else
        {
            logger.LogError("Workflow With Id: {WorkFlowId} was not found", notification.WorkFlowId);
            throw new NullReferenceException();
        }
    }
}
