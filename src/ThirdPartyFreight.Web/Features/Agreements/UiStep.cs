using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace ThirdPartyFreight.Web.Features.Agreements;

public sealed class UiStep
{
    public required string StepLabel { get; set; }
    public Func<Agreement, RenderFragment>? ChildContent { get; set; }
    public Action<WizardStepChangeEventArgs, Agreement>? OnChange { get; set; }
}
