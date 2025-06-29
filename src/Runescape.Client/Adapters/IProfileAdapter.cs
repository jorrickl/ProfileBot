using Runescape.Domain.Domain;

namespace Runescape.Client.Services
{
    public interface IProfileAdapter
    {
        Task<Profile?> GetProfileAsync(string user, int activities = 20);
    }
}