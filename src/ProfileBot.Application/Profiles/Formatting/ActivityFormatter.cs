using ProfileBot.Application.Interfaces;
using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Profiles.Formatting
{
    public class ActivityFormatter(IEnumerable<IActivityMatcher> matchers) : IActivityFormatter
    {
        public bool TryFormatActivities(Profile profile, out string? result)
        {
            result = null;
            if (profile.Activities.Length == 0)
            {
                return false;
            }

            var filteredActivities = new Dictionary<Activity, string>();
            foreach (var activity in profile.Activities)
            {
                foreach (var matcher in matchers)
                {
                    if (matcher.TryMatch(profile, activity, out var transformed) && !string.IsNullOrEmpty(transformed))
                    {
                        filteredActivities.Add(activity, transformed);
                        break;
                    }
                }
            }

            if (filteredActivities.Count == 0)
                return false;

            result = FormatOutput(filteredActivities);

            return true;
        }

        private static string FormatOutput(Dictionary<Activity, string> filtered)
        {
            var activities = from f in filtered
                             let unixTimestamp = DateTimeOffset.Parse(f.Key.Date).ToUnixTimeSeconds()
                             let formattedDate = $"<t:{unixTimestamp}:f>"
                             orderby unixTimestamp ascending
                             select $"- [{formattedDate}] ${f.Value}";

            return string.Join(Environment.NewLine, filtered);
        }
    }
}
