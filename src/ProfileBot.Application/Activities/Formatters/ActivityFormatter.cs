using ProfileBot.Application.Interfaces;
using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Activities.Formatters
{
    internal class ActivityFormatter : IActivityFormatter
    {
        public string FormatActivities(Profile profile)
        {
            var activities = from x in profile.Activities
                             let unixTimestamp = DateTimeOffset.Parse(x.Date).ToUnixTimeSeconds()
                             let formattedDate = $"<t:{unixTimestamp}:f>"
                             orderby unixTimestamp descending
                             select $"- [{formattedDate}] **{profile.Name}**: {x.Text}";

            return string.Join(Environment.NewLine, activities);
        }
    }
}
