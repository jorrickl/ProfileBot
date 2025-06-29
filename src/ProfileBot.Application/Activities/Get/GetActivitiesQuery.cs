using Ardalis.Result;
using ProfileBot.SharedKernel;

namespace ProfileBot.Application.Activities.Get
{
    public sealed record GetActivitiesQuery : IQuery<Result<GetActivitiesResult>>
    {
        public required string Username { get; init; }
        public required ulong GuildId { get; init; }
    }
}
