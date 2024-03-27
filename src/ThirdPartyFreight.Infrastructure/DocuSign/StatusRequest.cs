namespace ThirdPartyFreight.Infrastructure.DocuSign;

public sealed class StatusRequest
{
    public StatusRequest(
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
        UserFilter = userFilter;
        EnvelopeIds = envelopeIds;
        IncludeInformation = includeInformation;
        LastQueriedDate = lastQueriedDate;
        OrderBy = orderBy;
        FromStatus = fromStatus;
        StatusToInclude = statusToInclude;
        FromDate = fromDate;
        ToDate = toDate;
    }

    public string UserFilter { get; init; } = "sender";
    public string EnvelopeIds { get; init; } = string.Empty;
    public string IncludeInformation { get; init; } = "recipients";
    public string LastQueriedDate { get; init; } = string.Empty;
    public string OrderBy { get; init; } = "last_modified";
    public string FromStatus { get; init; } = "Changed";
    public string StatusToInclude { get; init; } = "completed,declined,delivered,processing,signed,voided";
    public string? FromDate { get; init; } = string.Empty;
    public string? ToDate { get; init; } = string.Empty;
}
