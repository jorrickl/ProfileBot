using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Interfaces
{
    public interface IActivityFormatter
    {
        bool TryFormatActivities(Profile profile, out string? result);
    }
}