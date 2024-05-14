namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public sealed record TaskPayload(ApprovalPayload Approval, string Description);
