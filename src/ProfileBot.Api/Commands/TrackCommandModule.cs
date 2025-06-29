using Ardalis.Result;
using MediatR;
using NetCord.Services.ApplicationCommands;
using ProfileBot.Application.Activities.Get;

namespace ProfileBot.Api.Commands
{
    [SlashCommand("track", "Track RuneMetrics profiles")]
    public class TrackCommandModule(IMediator mediator) : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SubSlashCommand("get", "Returns the latest activities for a specific player")]
        public async Task<string> GetActivities(
            [SlashCommandParameter(Name = "rsn", Description = "Player display name", MinLength = 1, MaxLength = 12)] string user)
        {
            var guildId = Context.Guild?.Id;
            if (!guildId.HasValue)
            {
                return "Could not obtain the server's ID.";
            }

            var query = new GetActivitiesQuery
            {
                Username = user,
                GuildId = guildId.Value
            };

            var result = await mediator.Send(query).ConfigureAwait(false);

            return result switch
            {
                { IsSuccess: true } => result.Value.Message,
                _ when result.IsNotFound() => result.Errors.Single(),
                _ => "Something went wrong. Try again."
            };
        }
    }
}
