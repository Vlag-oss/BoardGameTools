using System.ComponentModel.DataAnnotations;

namespace BoardGameTools.Application.Common.Options
{
    public sealed class TokenOptions
    {
        [Required]
        public string Issuer { get; init; } = string.Empty;
        [Required]
        public string Audience { get; init; } = string.Empty;
        [Range(1, 30)]
        public int AccessTokenExpirationMinutes { get; init; }
        [Range(1, 5)]
        public int RefreshTokenExpirationDays { get; init; }
        [Range(1, 30)]
        public double ResetPasswordTokenExpirationMinutes { get; init; }
        [Required, MinLength(32)]
        public string SigningKey { get; init; } = string.Empty;
        [Required, MinLength(32)]
        public string RefreshKey { get; init; } = string.Empty;
        
    }
}
