using ThirdPartyFreight.Domain.Agreements;
using FluentValidation;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

internal sealed class AddAgreementCommandValidator : AbstractValidator<AddAgreementCommand>
{
    public AddAgreementCommandValidator()
    {
        RuleFor(a => a.ContactEmail).EmailAddress();
        RuleFor(a => a.CustomerName).NotEmpty();
        RuleFor(a => a.CustomerNumber).GreaterThan(0);
        RuleFor(a => a.Status).IsInEnum();
        RuleFor(a => a.AgreementType).IsInEnum();
        RuleFor(a => a.SiteType).IsInEnum();
        RuleFor(a => a.ContactName).NotEmpty();
        RuleFor(a => a.CreatedBy).NotEmpty();
        RuleFor(a => a.Status).Equal(Status.Creating);
        RuleFor(a => a.AgreementType).NotEqual(AgreementType.Creating);
        RuleFor(a => a.SiteType).NotEqual(SiteType.Creating);
    }
}
