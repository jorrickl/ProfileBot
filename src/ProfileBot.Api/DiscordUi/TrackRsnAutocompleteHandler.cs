using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace ProfileBot.Api.DiscordUi
{
    internal class TrackRsnAutocompleteHandler : IAutocompleteProvider<AutocompleteInteractionContext>
    {
        private readonly string[] _users =
        {
            "Jorrick", "Suugataa", "Dark"
        };

        public ValueTask<IEnumerable<ApplicationCommandOptionChoiceProperties>?> GetChoicesAsync(ApplicationCommandInteractionDataOption option, AutocompleteInteractionContext context)
        {
            if (string.IsNullOrWhiteSpace(option.Value))
            {
                return new([]);
            }

            var matches = _users.Where(x => x.Contains(option.Value, StringComparison.OrdinalIgnoreCase))
                                .Take(10)
                                .Select(c => new ApplicationCommandOptionChoiceProperties(c, c));
            return new(matches);
        }
    }
}
