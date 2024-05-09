namespace ThirdPartyFreight.Application.Abstractions.DocuSign;

public sealed record StatusRequest(
    string UserFilter,
    string EnvelopeIds,
    string IncludeInformation,
    string OrderBy,
    string FromStatus,
    string StatusToInclude,
    string? FromDate,
    string? ToDate);
