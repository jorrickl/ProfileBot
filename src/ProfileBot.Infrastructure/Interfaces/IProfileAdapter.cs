using ProfileBot.Domain.Runescape;

namespace ProfileBot.Infrastructure.Interfaces
{
    public interface IProfileAdapter
    {
        Task<Profile?> GetProfileAsync(string user, int activities = 20);
    }
}