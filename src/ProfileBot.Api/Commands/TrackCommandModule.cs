using Ardalis.Result;
using MediatR;
using NetCord.Services.ApplicationCommands;
using ProfileBot.Api.DiscordUi;
using ProfileBot.Application.Activities.Get;
using ProfileBot.Application.Interfaces;

namespace ProfileBot.Api.Commands
{
    [SlashCommand("track", "Track RuneMetrics profiles")]
    public class TrackCommandModule(IMediator mediator, IActivityFormatter activityFormatter) : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SubSlashCommand("get", "Returns the latest activities for a specific player")]
        public async Task<string> GetActivities(
            [SlashCommandParameter(AutocompleteProviderType = typeof(ProfileNameAutocompleteHandler), MinLength = 1, MaxLength = 12)] string rsn)
        {
            var guildId = Context.Guild?.Id;
            if (!guildId.HasValue)
            {
                throw new InvalidOperationException("Could not obtain the server ID.");
            }

            var query = new GetProfileQuery
            {
                Username = rsn,
                GuildId = guildId.Value
            };

            var result = await mediator.Send(query).ConfigureAwait(false)
                      ?? throw new InvalidOperationException();

            if (result.IsSuccess && activityFormatter.TryFormatActivities(result.Value.UserProfile, out var formattedResult))
            {
                return formattedResult!;
            }

            return result switch
            {
                _ when result.IsInvalid() => FormatErrors(result.ValidationErrors.Select(x => x.ErrorMessage)),
                _ when result.IsNotFound() => FormatErrors(result.Errors),
                _ => "Something went wrong. Try again."
            };
        }


        private static string FormatErrors(IEnumerable<string> errors)
        {
            return string.Join(Environment.NewLine, errors);
        }
    }
}
