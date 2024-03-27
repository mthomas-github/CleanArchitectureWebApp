using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Documents.Events;

namespace ThirdPartyFreight.Domain.Documents;

public sealed class Document : Entity
{
    private Document(
        Guid id,
        Guid agreementId,
        Details document)
        : base(id)
    {
        AgreementId = agreementId;
        DocumentDetails = document;
    }

    private Document() { }
    public Guid AgreementId { get; private set; }
    public Details DocumentDetails { get; private set; }


    public static Document Create(
        Guid agreementId,
        Details documentDetails)
    {
        var document = new Document(Guid.NewGuid(), agreementId, documentDetails);

        document.RaiseDomainEvent(new DocumentCreatedDomainEvent(document.Id));

        return document;
    }
}