using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Messaging;
using ThirdPartyFreight.Domain.Abstractions;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;

internal sealed class AddWorkFlowTaskCommandHandler : ICommandHandler<AddWorkFlowTaskCommand, Guid>
{
    private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddWorkFlowTaskCommandHandler(IWorkFlowTaskRepository workFlowTaskRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _workFlowTaskRepository = workFlowTaskRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(AddWorkFlowTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var workFlowTask = WorkFlowTask.Create(request.ExternalId, request.ProcessId, request.Name,
                request.Description, request.AgreementId, _dateTimeProvider.UtcNow);

            _workFlowTaskRepository.Add(workFlowTask);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return workFlowTask.Id;
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(WorkFlowTaskErrors.CannotAdd);
        }
    }
}
