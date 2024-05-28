namespace ThirdPartyFreight.Infrastructure.Email;

public sealed class EmailOptions
{
    public string SmtpServer { get; init; }
    public string SmtpUserName { get; init; }
    public string SmtpPassword { get; init; }
    public int SmtpPort { get; init; }
    public string FromEmail { get; init; }
    
    public bool Ssl { get; init; }
}
