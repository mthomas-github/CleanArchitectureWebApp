namespace ThirdPartyFreight.Application.Abstractions.DocuSign;

public sealed class StatusRequest(
    string userFilter,
    string envelopeIds,
    string includeInformation,
    string lastQueriedDate,
    string orderBy,
    string fromStatus,
    string statusToInclude,
    string? fromDate,
    string? toDate)
{
    public string UserFilter { get; init; } = userFilter;
    public string EnvelopeIds { get; init; } = envelopeIds;
    public string IncludeInformation { get; init; } = includeInformation;
    public string LastQueriedDate { get; init; } = lastQueriedDate;
    public string OrderBy { get; init; } = orderBy;
    public string FromStatus { get; init; } = fromStatus;
    public string StatusToInclude { get; init; } = statusToInclude;
    public string? FromDate { get; init; } = fromDate;
    public string? ToDate { get; init; } = toDate;
}
