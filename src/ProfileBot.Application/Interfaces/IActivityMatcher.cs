using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Interfaces
{
    public interface IActivityMatcher
    {
        bool TryMatch(Profile profile, Activity activity, out string? result);
    }
}