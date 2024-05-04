using ThirdPartyFreight.Domain.Envelopes;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public class EnvelopeRequest
{
    public EnvelopeStatus EnvelopeStatus { get; set; }
    public string AgreementId { get; set; }
}
