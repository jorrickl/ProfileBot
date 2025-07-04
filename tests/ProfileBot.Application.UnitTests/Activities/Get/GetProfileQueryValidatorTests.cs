using FluentValidation.TestHelper;
using ProfileBot.Application.Profiles.Get;

namespace ProfileBot.Application.UnitTests.Activities.Get
{
    [TestClass]
    public class GetProfileQueryValidatorTests
    {
        private GetProfileQueryValidator _validator = null!;
        private GetProfileQuery _baseQuery = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new GetProfileQueryValidator();
            _baseQuery = new GetProfileQuery { Username = "ValidName1", GuildId = 1 };
        }

        [TestMethod]
        public void Validate_ValidUsername_ShouldPass()
        {
            var query = _baseQuery with { Username = "ValidName1" };
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void Validate_EmptyUsername_ShouldFail()
        {
            var query = _baseQuery with { Username = "" };
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameTooLong_ShouldFail()
        {
            var query = _baseQuery with { Username = "ThisNameIsWayTooLong" };
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithDoubleSpaces_ShouldFail()
        {
            var query = _baseQuery with { Username = "Bad  Name" };
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithInvalidCharacters_ShouldFail()
        {
            var query = _baseQuery with { Username = "Bad@Name!" };
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [TestMethod]
        public void Validate_UsernameWithMinLength_ShouldPass()
        {
            var query = _baseQuery with { Username = "A" };
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void Validate_UsernameWithMaxLength_ShouldPass()
        {
            var query = _baseQuery with { Username = "Abcdefghijkl" };
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
