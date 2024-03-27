using ThirdPartyFreight.Domain.Carriers;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Carriers.AddCarrier;

public sealed record AddCarrierCommand(
    Guid AgreementId,
    string CarrierName,
    string CarrierAccount,
    CarrierType CarrierType) : ICommand;
