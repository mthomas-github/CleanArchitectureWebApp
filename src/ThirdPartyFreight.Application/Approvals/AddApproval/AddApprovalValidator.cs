using FluentValidation;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

public class AddApprovalValidator : AbstractValidator<AddApprovalCommand>
{
    public AddApprovalValidator()
    {
        RuleFor(a => a.AgreementId).NotEmpty();
    }
}
