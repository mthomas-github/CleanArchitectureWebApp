using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Application.Approvals.UpdateApproval;

internal sealed class UpdateApprovalCommandHandler(IApprovalRepository approvalRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateApprovalCommand>
{
    public async Task<Result> Handle(UpdateApprovalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Approval? existingApproval = await approvalRepository.GetByIdAsync(request.ApprovalId, cancellationToken);

            if (existingApproval is null)
            {
                return Result.Failure(ApprovalErrors.NotFound);
            }

            var updatedApproval = Approval.Update(
                request.ApprovalId, 
                request.Approval.FirstApprovalOnUtc, 
                request.Approval.FirstApprovalEndUtc, 
                request.Approval.SecondApprovalOnUtc, 
                request.Approval.SecondApprovalEndUtc, 
                request.Approval.ThirdApprovalOnUtc, 
                request.Approval.ThirdApprovalEndUtc,
                request.Approval.CompletedOn);

            approvalRepository.Update(updatedApproval);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApprovalErrors.CannotUpdate);
        }
    }
}
