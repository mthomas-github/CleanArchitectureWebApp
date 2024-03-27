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

    public string UserFilter { get; init; }
    public string EnvelopeIds { get; init; }
    public string IncludeInformation { get; init; }
    public string LastQueriedDate { get; init; }
    public string OrderBy { get; init; }
    public string FromStatus { get; init; }
    public string StatusToInclude { get; init; }
    public string? FromDate { get; init; }
    public string? ToDate { get; init; }
}
