using ThirdPartyFreight.Domain.Carriers;

namespace ThirdPartyFreight.Api.Controllers.Carriers;

public sealed record AddCarrierRequest(Guid AgreementId, string CarrierName, string CarrierAccount, CarrierType CarrierType);
