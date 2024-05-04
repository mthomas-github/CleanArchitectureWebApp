using FluentValidation;

namespace ThirdPartyFreight.Application.Sites.AddSites;

internal sealed class SiteDetailValidator : AbstractValidator<SiteDetail>
{
    public SiteDetailValidator()
    {
        RuleFor(x => x.SiteNumber).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.ZipCode).NotEmpty();
    }
}
