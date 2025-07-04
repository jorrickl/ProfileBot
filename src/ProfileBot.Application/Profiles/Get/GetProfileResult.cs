using ProfileBot.Domain.Runescape;

namespace ProfileBot.Application.Profiles.Get
{
    public sealed record GetProfileResult
    {
        public required Profile UserProfile { get; init; }
    }
}
