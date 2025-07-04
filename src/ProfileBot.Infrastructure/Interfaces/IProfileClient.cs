using Ardalis.Result;
using ProfileBot.Domain.Runescape;

namespace ProfileBot.Infrastructure.Interfaces
{
    public interface IProfileClient
    {
        Task<Result<Profile>> GetProfileAsync(string user, int activities = 20);
    }
}