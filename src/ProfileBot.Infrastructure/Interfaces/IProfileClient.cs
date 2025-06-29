using ProfileBot.Domain.Runescape;

namespace ProfileBot.Infrastructure.Interfaces
{
    public interface IProfileClient
    {
        Task<Profile?> GetProfileAsync(string user, int activities = 20);
    }
}