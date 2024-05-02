using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace ThirdPartyFreight.Web.Features.Agreements.Models;

public sealed class UiStep
{
    public required string StepLabel { get; set; }
    public Func<AgreementResponse, RenderFragment>? ChildContent { get; set; }
    public Action<WizardStepChangeEventArgs, AgreementResponse>? OnChange { get; set; }
}
