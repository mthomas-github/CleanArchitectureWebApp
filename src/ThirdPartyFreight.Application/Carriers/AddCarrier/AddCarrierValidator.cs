using FluentValidation;

namespace ThirdPartyFreight.Application.Carriers.AddCarrier;

internal sealed class AddCarrierValidator : AbstractValidator<AddCarrierCommand>
{
    public AddCarrierValidator()
    {
        RuleFor(a => a.AgreementId).NotEmpty();
        RuleFor(a => a.CarrierName).NotEmpty();
        RuleFor(a => a.CarrierAccount).NotEmpty();
        RuleFor(a => a.CarrierType).IsInEnum();
    }
}
