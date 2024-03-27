using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Carriers.AddCarrier;

internal sealed class AddCarrierCommandHandler(ICarrierRepository carrierRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddCarrierCommand>
{
    public async Task<Result> Handle(AddCarrierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var carrier = Carrier.Create(
                request.AgreementId,
                new CarrierInfo(request.CarrierName, request.CarrierAccount, request.CarrierType));

            carrierRepository.Add(carrier);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(CarrierErrors.CannotAdd);
        }
    }
}
