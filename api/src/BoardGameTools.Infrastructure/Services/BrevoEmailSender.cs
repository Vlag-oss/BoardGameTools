using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Application.Common.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BoardGameTools.Infrastructure.Services
{
    public class BrevoEmailSender(IOptions<BrevoOptions> options) : IEmailSender
    {
        private readonly BrevoOptions _options = options.Value;

        public async Task SendAsync(string toEmail, string subject, string htmlContent, CancellationToken ct)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = htmlContent };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_options.SmtpServer, _options.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls, ct);
            await smtp.AuthenticateAsync(_options.SmtpUser, _options.SmtpKey, ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }
}
