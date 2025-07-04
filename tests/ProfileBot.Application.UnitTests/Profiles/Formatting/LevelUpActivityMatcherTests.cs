using ProfileBot.Application.Profiles.Formatting;
using ProfileBot.Domain.Runescape;
using Shouldly;

namespace ProfileBot.Application.UnitTests.Profiles.Formatting
{
    [TestClass]
    public class LevelUpActivityMatcherTests
    {
        private Profile _profile = null!;
        private const string _pattern = @"I levelled my (.*) skill, I am now level (\d.*)\.";

        [TestInitialize]
        public void Setup()
        {
            _profile = new Profile { Name = "TestUser" };
        }

        [DataRow("10")]
        [DataRow("20")]
        [DataRow("30")]
        [DataRow("40")]
        [DataRow("50")]
        [DataRow("60")]
        [DataRow("70")]
        [DataRow("80")]
        [DataRow("90")]
        [DataRow("110")]
        [DataTestMethod]
        public void TryMatch_WhenWhitelistedButNotMastered_ThenReturnsTrueWithoutTada(string level)
        {
            var matcher = new LevelUpActivityMatcher(_pattern, "{0} just levelled {1} to {2}");
            var activity = new Activity { Details = $"I levelled my Magic skill, I am now level {level}." };
            var matched = matcher.TryMatch(_profile, activity, out var result);
            matched.ShouldBeTrue();
            result.ShouldBe($"TestUser just levelled Magic to {level}");
        }

        [DataRow("99")]
        [DataRow("120")]
        [DataTestMethod]
        public void TryMatch_WhenWhitelistedButMastered_ThenReturnsTrueWithTada(string level)
        {
            var matcher = new LevelUpActivityMatcher(_pattern, "{0} just levelled {1} to {2}");
            var activity = new Activity { Details = $"I levelled my Magic skill, I am now level {level}." };
            var matched = matcher.TryMatch(_profile, activity, out var result);
            matched.ShouldBeTrue();
            result.ShouldBe($"TestUser just levelled Magic to {level} :tada:");
        }

        [TestMethod]
        public void TryMatch_WhenNonWhitelistedLevel_ThenReturnsFalse()
        {
            var matcher = new LevelUpActivityMatcher(_pattern, "{0} just levelled {1} to {2}");
            var activity = new Activity { Details = "Nonmatching details" };
            var matched = matcher.TryMatch(_profile, activity, out var result);
            matched.ShouldBeFalse();
            result.ShouldBeNull();
        }

        [TestMethod]
        public void TryMatch_WhenNoMatch_ThenReturnsFalse()
        {
            var matcher = new LevelUpActivityMatcher(_pattern, "{0} just levelled {1} to {2}");
            var activity = new Activity { Date = "2024-01-01", Details = "Level up: Magic (15)" };
            var matched = matcher.TryMatch(_profile, activity, out var result);
            matched.ShouldBeFalse();
            result.ShouldBeNull();
        }
    }
}
