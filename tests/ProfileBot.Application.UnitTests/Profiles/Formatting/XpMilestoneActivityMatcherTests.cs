using ProfileBot.Application.Profiles.Formatting;
using ProfileBot.Domain.Runescape;
using Shouldly;

namespace ProfileBot.Application.UnitTests.Profiles.Formatting
{
    [TestClass]
    public class XpMilestoneActivityMatcherTests
    {
        private Profile _profile = null!;
        private const string _pattern = @"XP: (\d+) (\w+)";
        private const string _output = "{0} reached {1}M XP in {2}";

        [TestInitialize]
        public void Setup()
        {
            _profile = new Profile { Name = "TestUser" };
        }

        [TestMethod]
        public void TryMatch_WhenCalled_ThenReturnsTrue()
        {
            var matcher = new XpMilestoneActivityMatcher(_pattern, _output);
            var activity = new Activity { Text = "XP: 10000000 Magic" };
            var matched = matcher.TryMatch(_profile, activity, out var result);
            matched.ShouldBeTrue();
            result.ShouldBe("TestUser reached 10M XP in Magic");
        }

        [TestMethod]
        public void TryMatch_WhenXpNotDivisibleByTenMillion_ThenReturnsFalse()
        {
            var matcher = new XpMilestoneActivityMatcher(_pattern, _output);
            var activity = new Activity { Text = "XP: 1234567 Magic" };
            var matched = matcher.TryMatch(_profile, activity, out var result);
            matched.ShouldBeFalse();
            result.ShouldBeNull();
        }
    }
}
