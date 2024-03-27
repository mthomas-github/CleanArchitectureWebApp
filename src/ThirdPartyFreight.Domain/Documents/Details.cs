namespace ThirdPartyFreight.Domain.Documents;

public record Details(
    string DocumentName,
    string DocumentData,
    DocumentType Type);