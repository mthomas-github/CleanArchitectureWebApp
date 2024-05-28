using System.Net.Mail;

namespace ThirdPartyFreight.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendEmailAsync(string recipient, string subject, string body);
    
    Task SendEmailAsync(string recipient, string subject, string body, Attachment[] attachments);
}
