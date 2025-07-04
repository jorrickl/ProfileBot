using ProfileBot.Application.Interfaces;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class ActivityMatcherFactory : IActivityMatcherFactory
    {
        public TMatcher Create<TMatcher>(string pattern, string output) where TMatcher : ActivityMatcher
            => (TMatcher)Activator.CreateInstance(typeof(TMatcher), pattern, output)!;
    }
}