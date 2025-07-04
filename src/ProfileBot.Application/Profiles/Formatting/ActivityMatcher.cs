using ProfileBot.Application.Interfaces;
using ProfileBot.Domain.Runescape;
using System.Text.RegularExpressions;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class ActivityMatcher(string pattern, string output) : IActivityMatcher
    {
        protected readonly string Pattern = pattern;
        protected readonly string Output = output;
        protected readonly Regex Regex = new(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public virtual bool TryMatch(Profile profile, Activity activity, out string? result)
        {
            result = null;
            var input = GetInput(profile, activity);
            var match = Regex.Match(input);
            if (match.Success && OnMatchSuccess(profile, activity, match, out result))
            {
                return true;
            }
            return false;
        }

        protected virtual string GetInput(Profile profile, Activity activity)
        {
            return activity.Text;
        }

        protected virtual bool OnMatchSuccess(Profile profile, Activity activity, Match match, out string? result)
        {
            result = Output;
            return true;
        }
    }
}
