using MediatR;
using Microsoft.Extensions.Logging;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Application.Abstractions.Hub;
using ThirdPartyFreight.Application.Approvals.GetApproval;
using ThirdPartyFreight.Application.Shared;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals.Events;

namespace ThirdPartyFreight.Application.Approvals.UpdateApproval;

public class ApprovalUpdatedDomainEventHandler(
    ISender sender,
    IApprovalClient approvalClient,
    ILogger<ApprovalUpdatedDomainEventHandler> logger
    ) : INotificationHandler<ApprovalUpdatedDomainEvent>
{

    public async Task Handle(ApprovalUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started To Handle {Name}", nameof(ApprovalUpdatedDomainEventHandler));
        // Step 1 Get Changed Approval from repo
        var getApprovalQuery = new GetApprovalQuery(notification.ApprovalId);
        Result<ApprovalResponse> results = await sender.Send(getApprovalQuery, cancellationToken);
        // Step 2 Error Handling Null Checking
        if (results.IsFailure)
        {
            logger.LogError("Approval Came Back Empty For Approval ID: {ApprovalId}", notification.ApprovalId);
            return;
        }
        ApprovalResponse updatedApproval = results.Value;
        // Step 3 Call Client Service To Fire Off Signal R To Client
        // Signal R will publish the change in real time not require client a round trip to the DB
        if (updatedApproval.CompletedOn is null && updatedApproval.Voided is null)
        {
            try
            {

                await approvalClient.SendPayload(updatedApproval, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("There was a problem, see error {ExMessage} ", ex.Message);
                throw new Exception(ex.Message);
            }
        } 
        else if (updatedApproval.Voided.HasValue && updatedApproval.Voided.Value)
        {
            try
            {
                await approvalClient.DeletePayload(updatedApproval.WorkFlowTaskId, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError("There was a problem, see error {ExMessage} ", ex.Message);
                throw new Exception(ex.Message);
            }
        }

        logger.LogInformation("Finished Handling {Name}", nameof(ApprovalUpdatedDomainEventHandler));
    }
}
