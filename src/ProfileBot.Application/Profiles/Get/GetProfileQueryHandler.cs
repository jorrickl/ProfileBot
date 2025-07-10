using Ardalis.Result;
using ProfileBot.Infrastructure.Interfaces;
using ProfileBot.SharedKernel;

namespace ProfileBot.Application.Profiles.Get
{
    public sealed class GetProfileQueryHandler(IProfileClient profileClient)
        : IQueryHandler<GetProfileQuery, Result<GetProfileResult>>
    {
        public async Task<Result<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            //TODO: Check if user is tracked for request.GuildId
            var profileResult = await profileClient.GetProfileAsync(request.Username).ConfigureAwait(false);

            if (profileResult.IsNotFound())
            {
                return Result.NotFound($"Profile not found for RSN: {request.Username}!");
            }
            else if (profileResult.IsError())
            {
                return Result<GetProfileResult>.Error(new ErrorList(profileResult.Errors));
            }

            return Result.Success(new GetProfileResult()
            {
                UserProfile = profileResult
            });
        }
    }
}
