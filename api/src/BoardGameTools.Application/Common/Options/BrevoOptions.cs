using System.ComponentModel.DataAnnotations;

namespace BoardGameTools.Application.Common.Options
{
    public sealed class BrevoOptions
    {
        [Required]
        public string FromEmail { get; init; } = string.Empty;
        [Required]
        public string FromName { get; init; } = string.Empty;
        [Required]
        public string SmtpServer { get; init; } = string.Empty;
        [Range(1, 65535)]
        public int SmtpPort { get; init; }
        [Required]
        public string SmtpUser { get; init; } = string.Empty;
        [Required]
        public string SmtpKey { get; init; } = string.Empty;
    }
}
