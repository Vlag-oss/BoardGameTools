using BoardGameTools.Application.Common.Interfaces;

namespace BoardGameTools.Application.Common.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
