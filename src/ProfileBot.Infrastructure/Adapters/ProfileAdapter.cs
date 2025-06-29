using ProfileBot.Domain.Runescape;
using ProfileBot.Infrastructure.Interfaces;

namespace ProfileBot.Infrastructure.Adapters
{
    internal class ProfileAdapter(IProfileClient profileClient) : IProfileAdapter
    {
        public Task<Profile?> GetProfileAsync(string user, int activities = 20)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(user);
            return profileClient.GetProfileAsync(user, activities);
        }
    }
}
