using BoardGameTools.Application.Common.Interfaces;
using BoardGameTools.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace BoardGameTools.Infrastructure.Services
{
    public class BrevoEmailSender(IConfiguration configuration) : IEmailSender
    {
        private readonly IConfigurationSection _brevoSection = configuration.GetRequiredSection("Brevo") ?? throw new ArgumentNullException("La section Brevo n'existe pas dans les paramètres");

        public async Task SendAsync(string toEmail, string subject, string htmlContent, CancellationToken ct)
        {
            var fromEmail = _brevoSection.GetRequiredValue<string>("FromEmail");
            var fromName = _brevoSection.GetRequiredValue<string>("FromName");
            var smtpServer = _brevoSection.GetRequiredValue<string>("SmtpServer");
            var smtpPort = _brevoSection.GetRequiredValue<int>("SmtpPort");
            var smtpUser = _brevoSection.GetRequiredValue<string>("SmtpUser");
            var smtpPass = _brevoSection.GetRequiredValue<string>("SmtpKey");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromName, fromEmail));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = htmlContent };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls, ct);
            await smtp.AuthenticateAsync(smtpUser, smtpPass, ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }
}
