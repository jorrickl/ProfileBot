using Microsoft.AspNetCore.WebUtilities;
using ProfileBot.Domain.Runescape;
using ProfileBot.Infrastructure.Interfaces;
using System.Text.Json;

namespace ProfileBot.Infrastructure.Clients
{
    internal class ProfileClient(HttpClient httpClient) : IProfileClient
    {
        private const string _profilePath = "/runemetrics/profile/profile";
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<Profile?> GetProfileAsync(string user, int activities = 20)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(user);

            var queryParameters = new Dictionary<string, string?>()
            {
                {  "user", Uri.EscapeDataString(user.ToLower()) },
                {  "activities", activities.ToString() }
            };

            var url = new Uri(httpClient.BaseAddress!, _profilePath).ToString();
            var urlWithQueryParameters = QueryHelpers.AddQueryString(url, queryParameters);

            var response = await httpClient.GetAsync(urlWithQueryParameters);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var profile = JsonSerializer.Deserialize<Profile?>(responseContent, _options);
            return profile;
        }
    }
}
