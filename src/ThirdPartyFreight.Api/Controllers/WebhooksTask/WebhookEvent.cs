namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public sealed record WebhookEvent(string EventType, Payload Payload, DateTimeOffset Timestamp);
