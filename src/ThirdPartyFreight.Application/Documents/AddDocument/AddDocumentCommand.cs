using ThirdPartyFreight.Domain.Documents;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Documents.AddDocument;

public sealed record AddDocumentCommand(
    Guid AgreementId,
    string DocumentName,
    string DocumentData,
    DocumentType Type) : ICommand;
