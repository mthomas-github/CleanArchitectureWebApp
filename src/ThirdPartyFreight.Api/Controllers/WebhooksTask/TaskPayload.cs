using ThirdPartyFreight.Domain.Approvals;

namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;

public sealed record TaskPayload(ApprovalPayload Approval, ApproverType approver);
