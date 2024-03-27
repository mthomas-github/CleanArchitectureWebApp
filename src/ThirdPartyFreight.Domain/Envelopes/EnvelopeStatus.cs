namespace ThirdPartyFreight.Domain.Envelopes;

public enum EnvelopeStatus
{
    NotRequired = 1,
    Draft = 2,
    Sent = 3,
    Delivered = 4,
    Declined = 5,
    Voided = 6,
    Completed = 7,
    Failed = 8
}
