using FluentValidation;
using FluentValidation.Validators;

namespace ThirdPartyFreight.Application.Sites.AddSites;

internal sealed class AddSitesValidator : AbstractValidator<AddSitesCommand>
{
    public AddSitesValidator()
    {
        RuleFor(x => x.AgreementId).NotEmpty();
        RuleFor(x => x.Sites).NotEmpty();
        RuleForEach(x => x.Sites).SetValidator(new SiteDetailValidator());
    }
}
