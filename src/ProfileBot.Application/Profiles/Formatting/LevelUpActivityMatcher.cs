using ProfileBot.Domain.Runescape;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class LevelUpActivityMatcher(string pattern, string output) : ActivityMatcher(pattern, output)
    {
        private readonly ImmutableList<short> _levelWhitelist =
        [
            10,
            20,
            30,
            40,
            50,
            60,
            70,
            80,
            90,
            99,
            110,
            120,
        ];

        protected override bool OnMatchSuccess(Profile profile, Activity activity, Match match, out string? result)
        {
            result = null;
            if (match.Groups.Count < 3)
                return false;

            var captureSkill = match.Groups[1].Value;
            var captureLevel = match.Groups[2].Value;

            if (short.TryParse(captureLevel, out var level) && _levelWhitelist.Contains(level))
            {
                result = string.Format(Output, profile.Name, captureLevel, captureSkill);

                if (level is 99 or 120)
                {
                    result += " :tada:";
                }
                return true;
            }
            return false;
        }

        protected override string GetInput(Profile _, Activity activity)
        {
            return activity.Details;
        }
    }
}
