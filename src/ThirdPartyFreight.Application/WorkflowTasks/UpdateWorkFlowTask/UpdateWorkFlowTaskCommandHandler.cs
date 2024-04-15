using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

internal sealed class UpdateWorkFlowTaskCommandHandler : ICommandHandler<UpdateWorkFlowTaskCommand>
{
    private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    public async Task<Result> Handle(UpdateWorkFlowTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            WorkFlowTask? existingWorkFlowTask =
                await _workFlowTaskRepository.GetByIdAsync(request.WorkFlowTaskId, cancellationToken);

            if (existingWorkFlowTask is null)
            {
                return Result.Failure(WorkFlowTaskErrors.NotFound);
            }

            var updated = WorkFlowTask.Update(
                request.WorkFlowTaskId,
                request.WorkflowTask.ExternalId,
                request.WorkflowTask.ProcessId,
                request.WorkflowTask.Name,
                request.WorkflowTask.Description,
                request.WorkflowTask.AgreementId);

            _workFlowTaskRepository.Update(updated);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(WorkFlowTaskErrors.CannotUpdate);
        }
    }
}
