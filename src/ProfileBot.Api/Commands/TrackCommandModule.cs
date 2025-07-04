using Ardalis.Result;
using MediatR;
using NetCord.Services.ApplicationCommands;
using ProfileBot.Api.DiscordUi;
using ProfileBot.Application.Activities.Get;

namespace ProfileBot.Api.Commands
{
    [SlashCommand("track", "Track RuneMetrics profiles")]
    internal class TrackCommandModule(IMediator mediator) : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SubSlashCommand("get", "Returns the latest activities for a specific player")]
        public async Task<string> GetActivities(
            [SlashCommandParameter(AutocompleteProviderType = typeof(ProfileNameAutocompleteHandler), MinLength = 1, MaxLength = 12)] string rsn)
        {
            var guildId = Context.Guild?.Id;
            if (!guildId.HasValue)
            {
                return "Could not obtain the server's ID.";
            }

            var query = new GetActivitiesQuery
            {
                Username = rsn,
                GuildId = guildId.Value
            };

            var result = await mediator.Send(query).ConfigureAwait(false);

            return result switch
            {
                { IsSuccess: true } => result.Value.Activities,
                _ when result.IsInvalid() => string.Join(Environment.NewLine, result.ValidationErrors.Select(x => x.ErrorMessage)),
                _ when result.IsNotFound() => result.Errors.First(),
                _ => "Something went wrong. Try again."
            };
        }
    }
}
