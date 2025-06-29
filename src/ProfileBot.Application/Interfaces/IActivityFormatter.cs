using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Interfaces
{
    public interface IActivityFormatter
    {
        string FormatActivities(Profile profile);
    }
}