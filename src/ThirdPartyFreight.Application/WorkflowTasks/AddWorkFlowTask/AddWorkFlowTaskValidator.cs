using FluentValidation;

namespace ThirdPartyFreight.Application.WorkflowTasks.AddWorkFlowTask;

internal sealed class AddWorkFlowTaskValidator : AbstractValidator<AddWorkFlowTaskCommand>
{
    public AddWorkFlowTaskValidator()
    {
        RuleFor(a => a.ExternalId).NotEmpty();
        RuleFor(a => a.ProcessId).NotEmpty();
        RuleFor(a => a.Name).NotEmpty();
        RuleFor(a => a.Description).NotEmpty();
        RuleFor(a => a.AgreementId).NotEmpty();
    }
}
