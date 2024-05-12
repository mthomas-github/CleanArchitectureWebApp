using System.Text.Json.Serialization;

namespace ThirdPartyFreight.Api.Controllers.WebhooksTask;


public abstract record ApprovalPayload(string AgreementId);
