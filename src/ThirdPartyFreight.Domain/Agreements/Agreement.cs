using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Agreements.Events;

namespace ThirdPartyFreight.Domain.Agreements;

public sealed class Agreement : Entity
{
    private Agreement(
        Guid id,
        ContactInfo contactInfo,
        Status status,
        AgreementType agreementType,
        SiteType siteType,
        DateTime createdOnUtc,
        CreatedBy createdBy,
        DateTime? modifiedOnUtc,
        ModifiedBy? modifiedBy,
        Ticket? ticket)
        : base(id)
    {
        ContactInfo = contactInfo;
        Status = status;
        AgreementType = agreementType;
        SiteType = siteType;
        CreatedBy = createdBy;
        CreatedOnUtc = createdOnUtc;
        ModifiedOnUtc = modifiedOnUtc;
        ModifiedBy = modifiedBy;
        Ticket = ticket;
    }

    private Agreement() {} // EF Core

    public ContactInfo ContactInfo { get; private set; }
    public Ticket? Ticket { get; private set; }
    public Status Status { get; private set; }
    public AgreementType AgreementType { get; private set; }
    public SiteType SiteType { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public CreatedBy CreatedBy { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }
    public ModifiedBy? ModifiedBy { get; private set; }

    
    public static Agreement Create(
        ContactInfo contactInfo,
        Status status,
        AgreementType agreementType,
        SiteType siteType,
        CreatedBy createdBy,
        DateTime utcNow)
    {
        var agreement = new Agreement(
            Guid.NewGuid(),
            contactInfo,
            status,
            agreementType,
            siteType,
            utcNow,
            createdBy,
            null,
            null,
            null);

        agreement.RaiseDomainEvent(new AgreementCreatedDomainEvent(agreement.Id));

        return agreement;
    }

    public Result Complete(DateTime utcNow, ModifiedBy modifiedBy)
    {
        if (Status != Status.Completed)
        {
            return Result.Failure(AgreementErrors.NotComplete);
        }

        Status = Status.Completed;
        ModifiedOnUtc = utcNow;
        ModifiedBy = modifiedBy;

        RaiseDomainEvent(new AgreementCompletedDomainEvent(Id));

        return Result.Success();
    }

    public static Agreement Update(
        Agreement agreement,
        Status status,
        Ticket? mdmTicket,
        ModifiedBy modifiedBy,
        DateTime utcNow)
    {
        var updated = new Agreement(
            agreement.Id,
            agreement.ContactInfo,
            status,
            agreement.AgreementType,
            agreement.SiteType,
            agreement.CreatedOnUtc,
            agreement.CreatedBy,
            utcNow,
            modifiedBy,
            mdmTicket);
        updated.RaiseDomainEvent(new AgreementUpdatedDomainEvent(updated.Id));

        return agreement;
    }
    

}
