using ProfileBot.Application.Profiles.Formatting;

namespace ProfileBot.Application.Interfaces
{
    public interface IActivityMatcherFactory
    {
        TMatcher Create<TMatcher>(string pattern, string output) where TMatcher : ActivityMatcher;
    }
}