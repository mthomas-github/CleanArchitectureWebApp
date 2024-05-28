using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ThirdPartyFreight.Application.Abstractions.Email;

namespace ThirdPartyFreight.Infrastructure.Email;

internal sealed class EmailService(ILogger<EmailService> logger, IOptions<EmailOptions> options) : IEmailService
{
    private readonly EmailOptions _options = options.Value;
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        var to = new MailAddress(recipient);
        var from = new MailAddress(_options.FromEmail);
        using var email = new MailMessage(from, to);
        email.Subject = subject;
        email.Body = body;

        using SmtpClient smtp = CreateSmtpClient();

        try
        {
            await smtp.SendMailAsync(email);
        }
        catch (SmtpException ex)
        {
            logger.LogError("Unable To Send Email. See Error {Error}", ex.Message);
        }
    }
    public async Task SendEmailAsync(string recipient, string subject, string body, Attachment[] attachments)
    {
        var to = new MailAddress(recipient);
        var from = new MailAddress(_options.FromEmail);
        using var email = new MailMessage(from, to);
        email.Subject = subject;
        email.Body = body;

        foreach (Attachment attachment in attachments)
        {
            email.Attachments.Add(attachment);
        }

        using SmtpClient smtp = CreateSmtpClient();

        try
        {
            await smtp.SendMailAsync(email);
        }
        catch (SmtpException ex)
        {
            logger.LogError("Unable To Send Email. See Error {Error}", ex.Message);
        }
    }

    private SmtpClient CreateSmtpClient()
    {
        var smtp = new SmtpClient
        {
            Host = _options.SmtpServer,
            Port = _options.SmtpPort,
            Credentials = new NetworkCredential(_options.SmtpUserName, _options.SmtpPassword),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = false
        };

        return smtp;
    }
}
