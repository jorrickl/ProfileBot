using ProfileBot.Application.Profiles.Formatting;
using ProfileBot.Domain.Runescape;
using Shouldly;

namespace ProfileBot.Application.UnitTests.Profiles.Formatting
{
    [TestClass]
    public class ActivityMatcherTests
    {
        private Profile _profile = null!;
        private Activity _activity = null!;
        private const string _pattern = "Level up: (\\w+)";

        [TestInitialize]
        public void Setup()
        {
            _profile = new Profile { Name = "TestUser" };
            _activity = new Activity { Date = "2024-01-01", Text = "Level up: Magic" };
        }

        [TestMethod]
        public void TryMatch_WhenPatternMatches_ReturnsTrueAndSetsResult()
        {
            var matcher = new ActivityMatcher(_pattern, "output");
            var matched = matcher.TryMatch(_profile, _activity, out var result);
            matched.ShouldBeTrue();
            result.ShouldBe("Matched: Level up: Magic");
        }

        [TestMethod]
        public void TryMatch_WhenPatternDoesNotMatch_ReturnsFalseAndNullResult()
        {
            var matcher = new ActivityMatcher("Achievement: (\\w+)", "output");
            var matched = matcher.TryMatch(_profile, _activity, out var result);
            matched.ShouldBeFalse();
            result.ShouldBeNull();
        }

        [TestMethod]
        public void TryMatch_WhenOnMatchSuccessReturnsFalse_ReturnsFalse()
        {
            var matcher = new ActivityMatcher(_pattern, "output");
            var matched = matcher.TryMatch(_profile, _activity, out var result);
            matched.ShouldBeFalse();
            result.ShouldBeNull();
        }
    }
}