using FluentValidation;

namespace ThirdPartyFreight.Application.Audits.AddAudit;

public class AddAuditValidator : AbstractValidator<AddAuditCommand>
{
    public AddAuditValidator()
    { 
        RuleFor(x => x.AgreementId).NotEmpty();
        RuleFor(x => x.AgreementId).NotEqual(Guid.Empty);
        RuleFor(x => x.AuditDateDue).NotNull();
    }
}
