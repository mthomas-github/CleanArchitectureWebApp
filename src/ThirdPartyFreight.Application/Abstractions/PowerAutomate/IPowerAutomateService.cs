namespace ThirdPartyFreight.Application.Abstractions.PowerAutomate;

public interface IPowerAutomateService
{
    Task TriggerFlow(Uri flowUrl, FlowRequest flowRequest);
}
