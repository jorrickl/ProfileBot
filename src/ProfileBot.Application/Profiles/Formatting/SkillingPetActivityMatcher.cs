using ProfileBot.Domain.Runescape;
using System.Text.RegularExpressions;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class SkillingPetActivityMatcher(string pattern, string output) : ActivityMatcher(pattern, output)
    {
        protected override bool OnMatchSuccess(Profile profile, Activity activity, Match match, out string? result)
        {
            result = null;
            if (match.Groups.Count < 3)
                return false;

            var capturePetName = match.Groups[1].Value;
            var captureSkillName = match.Groups[2].Value;

            result = string.Format(Output, profile.Name, capturePetName, captureSkillName);
            return true;
        }

        protected override string GetInput(Profile _, Activity activity)
        {
            return activity.Text;
        }
    }
}
