namespace ThirdPartyFreight.Infrastructure.Elsa;

public sealed class ElsaServerOptions
{
    public string ApiBaseUrl { get; init; }
    public string WfDefinitionId { get; init; }
    public string ApiKey { get; init; }
}
