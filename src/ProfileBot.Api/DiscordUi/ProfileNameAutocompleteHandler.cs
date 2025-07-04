using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace ProfileBot.Api.DiscordUi
{
    internal class ProfileNameAutocompleteHandler : IAutocompleteProvider<AutocompleteInteractionContext>
    {
        //TODO: Read the usernames via a repository by current guild
        private readonly string[] _users =
        [
            "Jorrick", "Suugataa", "Dark"
        ];

        public ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(ApplicationCommandInteractionDataOption option, AutocompleteInteractionContext context)
        {
            if (string.IsNullOrWhiteSpace(option.Value))
            {
                return new([]);
            }

            var matches = _users.Where(x => x.StartsWith(option.Value, StringComparison.OrdinalIgnoreCase))
                                .Take(10)
                                .Select(user => new ApplicationCommandOptionChoiceProperties(user, user));
            return new(matches);
        }
    }
}
