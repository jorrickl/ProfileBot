using Ardalis.Result;
using ProfileBot.SharedKernel;

namespace ProfileBot.Application.Profiles.Get
{
    public sealed record GetProfileQuery : IQuery<Result<GetProfileResult>>
    {
        public required string Username { get; init; }
        public ulong? GuildId { get; init; }
    }
}
