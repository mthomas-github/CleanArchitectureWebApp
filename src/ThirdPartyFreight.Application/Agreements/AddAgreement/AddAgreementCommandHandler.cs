using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

internal sealed class AddAgreementCommandHandler(
    IAgreementRepository agreementRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<AddAgreementCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddAgreementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var agreement = Agreement.Create(
                new ContactInfo(request.CustomerNumber, request.CustomerName, request.ContactName,
                    request.ContactEmail),
                request.Status,
                request.AgreementType,
                request.SiteType,
                request.CreatedBy,
                dateTimeProvider.UtcNow);

            agreementRepository.Add(agreement);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return agreement.Id;
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(AgreementErrors.NotComplete);
        }
    }
}
