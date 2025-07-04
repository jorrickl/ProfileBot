using ProfileBot.Application.Activities.Formatters;
using ProfileBot.Domain.Runescape;
using Shouldly;

namespace ProfileBot.Application.UnitTests.Activities.Formatters
{
    [TestClass]
    public class ActivityFormatterTests
    {
        private ActivityFormatter _formatter = null!;
        private Profile _baseProfile = null!;
        private string _date1 = null!;
        private string _date2 = null!;
        private long _unix1;
        private long _unix2;

        [TestInitialize]
        public void Setup()
        {
            _formatter = new ActivityFormatter();
            var dt1 = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var dt2 = new DateTime(2024, 1, 2, 15, 30, 0, DateTimeKind.Utc);
            _date1 = dt1.ToString("dd-MMM-yyyy HH:mmZ");
            _date2 = dt2.ToString("dd-MMM-yyyy HH:mmZ");
            _unix1 = new DateTimeOffset(dt1).ToUnixTimeSeconds();
            _unix2 = new DateTimeOffset(dt2).ToUnixTimeSeconds();
            _baseProfile = new Profile()
            {
                Name = "TestUser",
                Activities = [],
            };
        }

        [TestMethod]
        public void TryFormatActivities_NoActivities_ReturnsEmptyString()
        {
            var isFormatted = _formatter.TryFormatActivities(_baseProfile, out var _);

            isFormatted.ShouldBeFalse();
        }

        [TestMethod]
        public void TryFormatActivities_SingleActivity_FormatsCorrectly()
        {
            _baseProfile.Activities =
            [
                new Activity { Date = _date1, Details = "", Text = "Did something" }
            ];
            var expected = $"- [<t:{_unix1}:f>] **TestUser**: Did something";

            var isFormatted = _formatter.TryFormatActivities(_baseProfile, out var result);

            isFormatted.ShouldBeTrue();
            result.ShouldBe(expected);
        }

        [TestMethod]
        public void TryFormatActivities_MultipleActivities_OrdersByDateAscending()
        {
            _baseProfile.Activities =
            [
                new Activity { Date = _date1, Details = "", Text = "Old activity" },
                new Activity { Date = _date2, Details = "", Text = "New activity" }
            ];
            var expected = $"- [<t:{_unix1}:f>] **TestUser**: Old activity{Environment.NewLine}- [<t:{_unix2}:f>] **TestUser**: New activity";

            var isFormatted = _formatter.TryFormatActivities(_baseProfile, out var result);

            isFormatted.ShouldBeTrue();
            result.ShouldBe(expected);
        }

        [TestMethod]
        public void TryFormatActivities_ActivityWithInvalidDate_ThrowsFormatException()
        {
            _baseProfile.Activities =
            [
                new Activity { Date = "not-a-date", Details = "", Text = "Bad date" }
            ];

            var isFormatted = _formatter.TryFormatActivities(_baseProfile, out var _);
            isFormatted.ShouldBeFalse();
        }
    }
}