using System.ComponentModel.DataAnnotations;

namespace BoardGameTools.Application.Common.Options
{
    public sealed class AuthOptions
    {
        [Range(1, 5)]
        public int MaxFailed { get; init; }
        [Required]
        public TimeSpan AccountLockDuration { get; init; }
    }
}
