using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Activities.Get
{
    public sealed record GetProfileResult
    {
        public required Profile UserProfile { get; init; }
    }
}
