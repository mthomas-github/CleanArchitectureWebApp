namespace ThirdPartyFreight.Infrastructure.DocuSign;

public sealed class DocuSignOptions
{
    public string AuthUrl { get; init; } = string.Empty;
    public string IntegrationId { get; init; } = string.Empty;
    public string ImpersonatedUserId { get; init; } = string.Empty;
    public string PrivateKeyFilePath { get; init; } = string.Empty;
    public string TemplateId { get; init; } = string.Empty;
    public bool IsDevelopment { get; init; }
    public int IntervalInSeconds { get; init; }
    public int BatchSize { get; init; }
    
    public int ExpireInHoursCache { get; init; }
}
