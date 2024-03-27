using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Application.Shared;

namespace ThirdPartyFreight.Application.Documents.GetDocument;

public sealed record GetDocumentQuery(Guid DocumentId) : IQuery<DocumentResponse>;
