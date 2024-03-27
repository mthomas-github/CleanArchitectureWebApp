using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Approvals;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

public class AddApprovalCommandHandler(IApprovalRepository approvalRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    :ICommandHandler<AddApprovalCommand>
{
    public async Task<Result> Handle(AddApprovalCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var approval = Approval.Create(request.AgreementId, dateTimeProvider.UtcNow);

            approvalRepository.Add(approval);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(ApprovalErrors.CannotAdd);
        }
    }
}

