using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.UpdateWorkFlowTask;

internal sealed class UpdateWorkFlowTaskCommandHandler(IWorkFlowTaskRepository workFlowTaskRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateWorkFlowTaskCommand>
{
    public async Task<Result> Handle(UpdateWorkFlowTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            WorkFlowTask? existingWorkFlowTask =
                await workFlowTaskRepository.GetByIdAsync(request.WebHookTaskId, cancellationToken);

            if (existingWorkFlowTask is null)
            {
                return Result.Failure(WorkFlowTaskErrors.NotFound);
            }

            if (request.Voided)
            {
                var denied = WorkFlowTask.Decline(existingWorkFlowTask);
                workFlowTaskRepository.Update(denied);
            }
            else
            {
                var updated = WorkFlowTask.Complete(existingWorkFlowTask);
                workFlowTaskRepository.Update(updated);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(WorkFlowTaskErrors.CannotUpdate);
        }
    }
}
