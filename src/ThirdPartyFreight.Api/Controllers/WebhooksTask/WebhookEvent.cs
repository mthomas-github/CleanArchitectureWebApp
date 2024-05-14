namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public sealed record WebhookEvent(string EventType, RunTaskWebhook Payload, DateTimeOffset Timestamp);
