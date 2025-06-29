using FluentValidation;

namespace ProfileBot.Application.Activities.Get
{
    internal class GetActivitiesQueryValidator : AbstractValidator<GetActivitiesQuery>
    {
        public GetActivitiesQueryValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(12)
                .Matches("^(?!.*  )[A-Za-z0-9](?:[A-Za-z0-9 ]{0,10}[A-Za-z0-9])?$")
                .WithMessage("Invalid value for {PropertyName}");
        }
    }
}
