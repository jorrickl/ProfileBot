using ProfileBot.Domain.Runescape;
using System.Text.RegularExpressions;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class SingleTextActivityMatcher(string pattern, string output) : ActivityMatcher(pattern, output)
    {
        protected override bool OnMatchSuccess(Profile profile, Activity activity, Match match, out string? result)
        {
            result = null;
            if (match.Groups.Count < 2)
                return false;

            var capture = match.Groups[1].Value;

            result = string.Format(Output, profile.Name, capture);
            return true;
        }

        protected override string GetInput(Profile _, Activity activity)
        {
            return activity.Text;
        }
    }
}
