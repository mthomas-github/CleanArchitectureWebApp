namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public abstract record TaskPayload(ApprovalPayload Approval, string Description);
