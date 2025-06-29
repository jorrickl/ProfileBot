using Runescape.Client.Clients;
using Runescape.Domain.Domain;

namespace Runescape.Client.Services
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
