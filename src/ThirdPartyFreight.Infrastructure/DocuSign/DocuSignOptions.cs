namespace ThirdPartyFreight.Infrastructure.DocuSign;

public sealed class DocuSignOptions
{
    public string AuthUrl { get; set; } = string.Empty;
    public string IntegrationId { get; set; } = string.Empty;
    public string ImpersonatedUserId { get; set; } = string.Empty;
    public string PrivateKeyFilePath { get; set; } = string.Empty;
    public string TemplateId { get; set; } = string.Empty;
    public bool IsDevelopment { get; set; }
}
