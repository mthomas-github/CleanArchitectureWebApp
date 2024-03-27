using FluentValidation;

namespace ThirdPartyFreight.Application.Envelopes.AddEnvelope;

internal sealed class AddEnvelopeValidator : AbstractValidator<AddEnvelopeCommand>
{
    public AddEnvelopeValidator()
    {
        RuleFor(x => x.AgreementId).NotEmpty();
        RuleFor(x => x.EnvelopeStatus).IsInEnum();
    }
}
