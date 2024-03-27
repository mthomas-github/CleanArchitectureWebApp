using FluentValidation;

namespace ThirdPartyFreight.Application.Approvals.AddApproval;

internal sealed class AddApprovalValidator : AbstractValidator<AddApprovalCommand>
{
    public AddApprovalValidator()
    {
        RuleFor(a => a.AgreementId).NotEmpty();
    }
}
