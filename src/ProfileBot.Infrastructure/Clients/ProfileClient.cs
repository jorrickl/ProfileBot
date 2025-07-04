using Ardalis.Result;
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

        public async Task<Result<Profile>> GetProfileAsync(string user, int activities = 20)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(user);

            var queryParameters = new Dictionary<string, string?>()
            {
                {  "user", Uri.EscapeDataString(user) },
                {  "activities", activities.ToString() }
            };

            var url = new Uri(httpClient.BaseAddress!, _profilePath).ToString();
            var urlWithQueryParameters = QueryHelpers.AddQueryString(url, queryParameters);

            var response = await httpClient.GetAsync(urlWithQueryParameters);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Error("Something went wrong while contacting the RuneScape API.");
            }

            // Try to deserialize as Profile
            var profile = JsonSerializer.Deserialize<Profile?>(responseContent, _options);
            if (profile != null && !string.IsNullOrWhiteSpace(profile.Name))
            {
                return Result.Success(profile);
            }

            // If not a valid profile, try to deserialize as ErrorResponse
            var error = JsonSerializer.Deserialize<ErrorResponse?>(responseContent, _options);
            if (error != null && !string.IsNullOrWhiteSpace(error.Error))
            {
                return Result.Error(error.Error);
            }

            // Unknown response
            return Result.Error("Unknown response from RuneMetrics API.");
        }
    }
}
