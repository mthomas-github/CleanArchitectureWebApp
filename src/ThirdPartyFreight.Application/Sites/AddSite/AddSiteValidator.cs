using FluentValidation;

namespace ThirdPartyFreight.Application.Sites.AddSite;

internal sealed class AddSiteValidator : AbstractValidator<AddSiteCommand>
{
    public AddSiteValidator()
    {
        RuleFor(x => x.AgreementId).NotEmpty();
        RuleFor(x => x.SiteNumber).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.SiteNumber).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
    }
}
