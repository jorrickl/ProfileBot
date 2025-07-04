using Ardalis.Result;
using MediatR;
using NetCord.Services.ApplicationCommands;
using ProfileBot.Api.DiscordUi;
using ProfileBot.Application.Interfaces;
using ProfileBot.Application.Profiles.Get;

namespace ProfileBot.Api.Commands
{
    [SlashCommand("track", "Track RuneMetrics profiles")]
    public class TrackSlashCommand(IMediator mediator, IActivityFormatter activityFormatter) : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SubSlashCommand("get", "Returns the latest activities for a specific player")]
        public async Task<string> GetActivities(
            [SlashCommandParameter(AutocompleteProviderType = typeof(ProfileNameAutocompleteHandler), MinLength = 1, MaxLength = 12)] string rsn)
        {
            var query = new GetProfileQuery
            {
                Username = rsn,
                GuildId = Context.Guild?.Id
            };
            var profile = await mediator.Send(query).ConfigureAwait(false)
                       ?? throw new InvalidOperationException();

            if (profile.IsSuccess && activityFormatter.TryFormatActivities(profile.Value.UserProfile, out var formattedResponse))
            {
                return formattedResponse!;
            }

            var errorResponse = profile switch
            {
                _ when profile.IsInvalid() => FormatErrors(profile.ValidationErrors.Select(x => x.ErrorMessage)),
                _ when profile.IsNotFound() => FormatErrors(profile.Errors),
                _ => "Something went wrong. Try again later."
            };

            return errorResponse;
        }

        private static string FormatErrors(IEnumerable<string> errors)
        {
            return string.Join(Environment.NewLine, errors);
        }
    }
}
