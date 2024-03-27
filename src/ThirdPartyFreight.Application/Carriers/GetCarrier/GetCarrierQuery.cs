using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Carriers.GetCarrier;

public sealed record GetCarrierQuery(Guid CarrierId) : IQuery<CarrierResponse>;
