using ThirdPartyFreight.Application.Approvals.AddApproval;

namespace ThirdPartyFreight.Application.Abstractions.Elsa;

public interface IElsaService
{
    Task ExecuteTask(string agreementId, CancellationToken cancellationToken);
    Task CompleteTask(string taskId, object? result = default, CancellationToken cancellationToken = default);
    Task DeleteWfInstance(string processId,
        CancellationToken cancellationToken = default);
}
