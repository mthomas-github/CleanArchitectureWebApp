using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Domain.Approvals.Events;

namespace ThirdPartyFreight.Application.Approvals.UpdateApproval;

public class ApprovalUpdatedDomainEventHandler(
    IApprovalRepository approvalRepository,
    IApprovalClient approvalClient,
    ILogger<ApprovalUpdatedDomainEventHandler> logger
    ) : INotificationHandler<ApprovalUpdatedDomainEvent>
{

    public async Task Handle(ApprovalUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started To Handle {Name}", nameof(ApprovalUpdatedDomainEventHandler));
        // Step 1 Get Changed Approval from repo
        Approval? updatedApproval = await approvalRepository.GetByIdAsync(notification.ApprovalId, cancellationToken);
        // Step 2 Error Handling Null Checking
        if (updatedApproval is null)
        {
            logger.LogError("Approval Came Back Empty For Approval ID: {ApprovalId}", notification.ApprovalId);
            return;
        }
        
        // Step 3 Call Client Service To Fire Off Signal R To Client
        // Signal R will publish the change in real time not require client a round trip to the DB
        try
        {
            await approvalClient.SendPayload(updatedApproval, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("There was a problem, see error {ExMessage} ", ex.Message);
            throw new Exception(ex.Message);
        }

        logger.LogInformation("Finish Handling {Name}, as Completed", nameof(ApprovalUpdatedDomainEventHandler));
    }
}
