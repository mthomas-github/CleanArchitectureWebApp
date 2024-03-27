using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Envelopes;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Envelopes.AddEnvelope;

internal sealed class AddEnvelopeCommandHandler(
    IEnvelopeRepository envelopeRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddEnvelopeCommand>
{
    public async Task<Result> Handle(AddEnvelopeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var envelope = Envelope.Create(request.EnvelopeStatus, request.AgreementId, dateTimeProvider.UtcNow);

            envelopeRepository.Add(envelope);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(EnvelopeErrors.CannotAdd);
        }
    }
}
