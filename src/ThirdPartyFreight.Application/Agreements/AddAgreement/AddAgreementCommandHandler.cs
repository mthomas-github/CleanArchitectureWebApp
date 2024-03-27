using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Agreements.AddAgreement;

internal sealed class AddAgreementCommandHandler : ICommandHandler<AddAgreementCommand, Guid>
{
    private readonly IAgreementRepository _agreementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddAgreementCommandHandler(
        IAgreementRepository agreementRepository, 
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider)
    {
        _agreementRepository = agreementRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

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
                _dateTimeProvider.UtcNow);

            _agreementRepository.Add(agreement);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return agreement.Id;
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(AgreementErrors.NotComplete);
        }
    }
}
