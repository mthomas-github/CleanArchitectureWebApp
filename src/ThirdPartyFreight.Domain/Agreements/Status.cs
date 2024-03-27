namespace ThirdPartyFreight.Domain.Agreements;

public enum Status
{
    Creating = 1,
    CustomerResponse = 2,
    CustomerSignature = 3,
    PendingReviewTpf = 4,
    PendingReviewTms = 5,
    PendingReviewMdm = 6,
    Completed = 7,
    Closed = 8,
    Failed = 9,
    Cancelled = 10,
}