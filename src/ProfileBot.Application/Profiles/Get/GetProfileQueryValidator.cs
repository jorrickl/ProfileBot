using FluentValidation;

namespace ProfileBot.Application.Profiles.Get
{
    public class GetProfileQueryValidator : AbstractValidator<GetProfileQuery>
    {
        private const string _displayNameRegex = "^(?!.*  )[A-Za-z0-9](?:[A-Za-z0-9 ]{0,10}[A-Za-z0-9])?$";

        public GetProfileQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(12)
                .Matches(_displayNameRegex)
                .WithMessage("Invalid value for {PropertyName}");
        }
    }
}
