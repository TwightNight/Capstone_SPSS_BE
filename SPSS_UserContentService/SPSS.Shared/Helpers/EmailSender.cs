using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SPSS.Shared.Helpers;

public class EmailSender
{
    private readonly IConfiguration _config;
    public EmailSender(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var emailConfig = _config.GetSection("Email");
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(emailConfig["FromName"], emailConfig["FromEmail"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        var builder = new BodyBuilder { HtmlBody = htmlBody };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(emailConfig["SmtpHost"], int.Parse(emailConfig["SmtpPort"]), MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(emailConfig["User"], emailConfig["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
