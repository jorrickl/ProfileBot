using ProfileBot.Application.Interfaces;
using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Activities.Formatters
{
    public class ActivityFormatter : IActivityFormatter
    {
        public bool TryFormatActivities(Profile profile, out string? result)
        {
            result = null!;
            if (profile.Activities.Length == 0)
            {
                return false;
            }

            try
            {
                var activities = from x in profile.Activities
                                 let unixTimestamp = DateTimeOffset.Parse(x.Date).ToUnixTimeSeconds()
                                 let formattedDate = $"<t:{unixTimestamp}:f>"
                                 orderby unixTimestamp ascending
                                 select $"- [{formattedDate}] **{profile.Name}**: {x.Text}";
                result = string.Join(Environment.NewLine, activities);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
