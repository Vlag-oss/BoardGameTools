using System.ComponentModel.DataAnnotations;

namespace BoardGameTools.Application.Common.Options
{
    public sealed class ClientOptions
    {
        [Required]
        public string BaseUrl { get; init; } = string.Empty;
    }
}
