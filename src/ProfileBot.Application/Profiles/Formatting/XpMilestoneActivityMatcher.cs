using ProfileBot.Domain.Runescape;
using System.Text.RegularExpressions;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class XpMilestoneActivityMatcher(string pattern, string output) : ActivityMatcher(pattern, output)
    {
        protected override bool OnMatchSuccess(Profile profile, Activity activity, Match match, out string? result)
        {
            result = null;
            if (match.Groups.Count < 3)
                return false;

            var captureXp = match.Groups[1].Value;
            var captureSkill = match.Groups[2].Value;

            if (long.TryParse(captureXp, out var xp) && xp % 10_000_000L == 0)
            {
                var outputXp = FormatXp(xp);
                var outputSkill = FormatSkill(captureSkill);
                result = string.Format(Output, profile.Name, outputXp, outputSkill);
                return true;
            }
            return false;
        }

        private static string FormatXp(long xp)
        {
            return $"{xp / 1_000_000L}";
        }

        public static string FormatSkill(string input)
            => char.ToUpperInvariant(input[0]) + input[1..].ToLowerInvariant();
    }
}
