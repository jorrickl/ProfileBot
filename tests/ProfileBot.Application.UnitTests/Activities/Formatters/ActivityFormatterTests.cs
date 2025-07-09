using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using ProfileBot.Application.Interfaces;
using ProfileBot.Application.Profiles.Formatting;
using ProfileBot.Domain.Runescape;
using Shouldly;

namespace ProfileBot.Application.UnitTests.Activities.Formatters
{
    [TestClass]
    public class ActivityFormatterTests
    {
        private ActivityFormatter _formatter = null!;
        private IEnumerable<Mock<IActivityMatcher>> _matchersMock = null!;
        private Profile _baseProfile = null!;
        private string _date1 = null!;
        private string _date2 = null!;
        private long _unix1;
        private long _unix2;

        [TestInitialize]
        public void Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _matchersMock = fixture.CreateMany<Mock<IActivityMatcher>>();
            fixture.Inject(_matchersMock.Select(x => x.Object));
            _formatter = fixture.Create<ActivityFormatter>();
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
        public void TryFormatActivities_MultipleActivities_OrdersByDateAscending()
        {
            // Arrange
            var activity1 = new Activity { Date = _date1, Details = "", Text = "Old activity" };
            var activity2 = new Activity { Date = _date2, Details = "", Text = "New activity" };
            _baseProfile.Activities = [activity1, activity2];

            var result1 = "**TestUser**: Old activity";
            var result2 = "**TestUser**: New activity";
            _matchersMock.First().Setup(x => x.TryMatch(_baseProfile, activity1, out result1)).Returns(true);
            _matchersMock.First().Setup(x => x.TryMatch(_baseProfile, activity2, out result2)).Returns(true);

            var expectedResult = $"- [<t:{_unix1}:f>] **TestUser**: Old activity{Environment.NewLine}- [<t:{_unix2}:f>] **TestUser**: New activity";

            // Act
            var isFormatted = _formatter.TryFormatActivities(_baseProfile, out var result);

            // Assert
            isFormatted.ShouldBeTrue();
            result.ShouldBe(expectedResult);
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