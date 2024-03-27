using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.Audits;
using ThirdPartyFreight.Application.Abstractions.Messaging;

namespace ThirdPartyFreight.Application.Audits.AddAudit;

internal sealed class AddAuditCommandHandler(IAuditRepository auditRepository, IUnitOfWork unitOfWork)
    :ICommandHandler<AddAuditCommand>
{
    public async Task<Result> Handle(AddAuditCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var audit = Audit.Create(request.AgreementId, new AuditInfo(request.AuditDateDue, request.IsAuditActive, null));

            auditRepository.Add(audit);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(AuditErrors.CannotAdd);
        }
    }
}
