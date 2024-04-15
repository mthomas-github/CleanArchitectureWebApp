namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public class WebHookTaskRequest
{
    public Guid Id { get; set; }
    public string ProcessId { get; set; }
    public string ExternalId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid AgreementId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }
}
