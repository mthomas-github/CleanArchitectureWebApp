using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace ThirdPartyFreight.Web.Features.Agreements;

public sealed class UiStep
{
    public required string StepLabel { get; set; }
    public Func<GetAgreement, RenderFragment>? ChildContent { get; set; }
    public Action<WizardStepChangeEventArgs, GetAgreement>? OnChange { get; set; }
}
