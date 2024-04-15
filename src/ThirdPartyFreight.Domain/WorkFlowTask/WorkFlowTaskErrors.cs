using ThirdPartyFreight.Domain.Abstractions;

namespace ThirdPartyFreight.Domain.WorkflowTask;

public static class WorkFlowTaskErrors
{
    public static readonly Error NotFound = new(
               "WorkFlowTask.NotFound",
                      "The workflow task with the specified identifier was not found");

    public static readonly Error CannotAdd = new(
                      "WorkFlowTask.CannotAdd",
                                           "The workflow task was unable to be added.");

    public static Error CannotUpdate = new(
                             "WorkFlowTask.CannotUpdate",
                                                                       "The workflow task was unable to be updated.");
}
